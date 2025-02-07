using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Chat.Exporting;
using AppFramework.Dto;
using AppFramework.EntityFrameworkCore;
using AppFramework.MultiTenancy;

namespace AppFramework.Gdpr
{
    public class ChatUserCollectedDataProvider : IUserCollectedDataProvider, ITransientDependency
    {
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IChatMessageListExcelExporter _chatMessageListExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<UserAccount, long> _userAccountRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IObjectMapper _objectMapper;

        public ChatUserCollectedDataProvider(
            IRepository<ChatMessage, long> chatMessageRepository,
            IChatMessageListExcelExporter chatMessageListExcelExporter,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserAccount, long> userAccountRepository,
            IRepository<Tenant> tenantRepository,
            IObjectMapper objectMapper)
        {
            _chatMessageRepository = chatMessageRepository;
            _chatMessageListExcelExporter = chatMessageListExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _userAccountRepository = userAccountRepository;
            _tenantRepository = tenantRepository;
            _objectMapper = objectMapper;
        }

        public async Task<List<FileDto>> GetFiles(UserIdentifier user)
        {
            var conversations = await GetUserChatMessages(user.TenantId, user.UserId);

            Dictionary<UserIdentifier, string> relatedUsernames;
            Dictionary<int, string> relatedTenancyNames;

            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var tenantIds = conversations.Select(c => c.Key.TenantId);
                relatedTenancyNames = _tenantRepository.GetAll().Where(t => tenantIds.Contains(t.Id)).ToDictionary(t => t.Id, t => t.TenancyName);
                relatedUsernames = GetFriendUsernames(conversations.Select(c => c.Key).ToList());
            }

            var chatMessageFiles = new List<FileDto>();

            foreach (var conversation in conversations)
            {
                foreach (var message in conversation.Value)
                {
                    message.TargetTenantName = message.TargetTenantId.HasValue
                        ? relatedTenancyNames[message.TargetTenantId.Value]
                        : ".";

                    message.TargetUserName = relatedUsernames[new UserIdentifier(message.TargetTenantId, message.TargetUserId)];
                }

                var messages = conversation.Value.OrderBy(m => m.CreationTime).ToList();
                chatMessageFiles.Add(_chatMessageListExcelExporter.ExportToFile(user, messages));
            }

            return chatMessageFiles;
        }

        private Dictionary<UserIdentifier, string> GetFriendUsernames(List<UserIdentifier> users)
        {
            var predicate = PredicateBuilder.New<UserAccount>();

            foreach (var user in users)
            {
                predicate = predicate.Or(ua => ua.TenantId == user.TenantId && ua.UserId == user.UserId);
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var userList = _userAccountRepository.GetAllList(predicate).Select(ua => new
                {
                    ua.TenantId,
                    ua.UserId,
                    ua.UserName
                }).Distinct();

                return userList.ToDictionary(ua => new UserIdentifier(ua.TenantId, ua.UserId), ua => ua.UserName);
            }
        }

        private async Task<Dictionary<UserIdentifier, List<ChatMessageExportDto>>> GetUserChatMessages(int? tenantId, long userId)
        {
            var conversations = (await _chatMessageRepository.GetAll()
                    .Where(message => message.UserId == userId && message.TenantId == tenantId)
                    .ToListAsync()
                )
                .GroupBy(message => new {message.TargetTenantId, message.TargetUserId})
                .Select(messageGrouped => new
                {
                    TargetTenantId = messageGrouped.Key.TargetTenantId,
                    TargetUserId = messageGrouped.Key.TargetUserId,
                    Messages = messageGrouped
                }).ToList();

            return conversations.ToDictionary(c => new UserIdentifier(c.TargetTenantId, c.TargetUserId), c => _objectMapper.Map<List<ChatMessageExportDto>>(c.Messages));
        }
    }
}