using BottApp.Database;
using BottApp.Database.Service;
using BottApp.Database.Service.Keyboards;
using BottApp.Database.User;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace BottApp.Host.Services.Handlers.Auth
{
    public class AuthHandler : IAuthHandler
    {
      
        private readonly IUserRepository _userRepository;
        private readonly IMessageService _messageService;


        public AuthHandler(IUserRepository userRepository, IMessageService messageService)
        {
            _userRepository = userRepository;
            _messageService = messageService;
        }

        public async Task BotOnMessageReceived(
            ITelegramBotClient botClient,
            Message message,
            CancellationToken cancellationToken,
            UserModel user
        )
        {
      
            
            await _messageService.MarkMessageToDelete(message);
            
            if (message.Text == "/start" && user.Phone.IsNullOrEmpty())
            {
                await SendMainPicture(botClient, message);
                await BotWelcomeMessage(botClient, message, cancellationToken);
                return;
            }


            if (user.Phone.IsNullOrEmpty())
            {
                if (message.Contact != null)
                {
                    await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);

                    await _messageService.MarkMessageToDelete(
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id, text: "Cпасибо!\nТеперь отправь свое имя",
                            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken
                        )
                    );

                    await _userRepository.UpdateUserPhone(user, message.Contact.PhoneNumber);
                    return;
                }
                await Task.Delay(1000);

                await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);

                await _messageService.MarkMessageToDelete(
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                        text: "Отправьте телефон по кнопке 'Поделиться контактом'",
                        replyMarkup: Keyboard.RequestLocationAndContactKeyboard)
                );
                return;
                 
            }

            if (user.FirstName.IsNullOrEmpty() && !user.Phone.IsNullOrEmpty())
            {
                if (message.Text != null)
                {
                    await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);

                    await _messageService.MarkMessageToDelete(
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id, text: "Cпасибо!\nТеперь отправьте фамилию"
                        )
                    );
                    user.FirstName = message.Text;
                    return;
                }

                await Task.Delay(1000);

                await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);

                await _messageService.MarkMessageToDelete(
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Отправьте имя в виде текста"));
                return;
            }

            if (user.LastName.IsNullOrEmpty() && !user.FirstName.IsNullOrEmpty())
            {
                if (message.Text != null)
                {
                    await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);

                    user.LastName = message.Text;
                    
                    var profile = new Profile(user.FirstName, user.LastName);

                    await _userRepository.UpdateUserFullName(user, profile);

                    await SendUserFormToAdmin(botClient, message, user);
                    
                    await _messageService.MarkMessageToDelete(
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Отлично!\nПередал заявку на модерацию.\nОжидайте уведомление :)"
                        )
                    );
                    await Task.Delay(4000, cancellationToken);
                    await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);
                    return;
                }

                await Task.Delay(1000);

                await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);

                await _messageService.MarkMessageToDelete(
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id, text: "Отправьте фамилию в виде текста"
                    )
                );
                return;
            }
            
          
            
            if (message.Text != null && !user.LastName.IsNullOrEmpty() && !user.FirstName.IsNullOrEmpty() && !user.Phone.IsNullOrEmpty())
            {
                await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);
            
                await _messageService.MarkMessageToDelete(
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id, text: "Ваши данные на проверке, не переживайте!"
                    )
                );
                await Task.Delay(2000);
                await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);
                return;
            }
            
            await _messageService.DeleteMessages(botClient, user.UId, message.MessageId);
            
            // await _messageService.MarkMessageToDelete(
            //     await botClient.SendTextMessageAsync(
            //         chatId: message.Chat.Id, text: "Если все сломалось - тыкай /start"));
        }


        public async Task BotWelcomeMessage(
            ITelegramBotClient botClient,
            Message? message,
            CancellationToken cancellationToken
        )
        {
            await _messageService.MarkMessageToDelete(
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Привет! Я чат бот - Чатик",
                    cancellationToken: cancellationToken
                )
            );
            
            await Task.Delay(1500);
            
            await _messageService.MarkMessageToDelete(
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Давайте знакомиться!",
                    cancellationToken: cancellationToken
                )
            );
            
            await Task.Delay(1500);
            
            await _messageService.MarkMessageToDelete(
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text:
                    "Для начала поделитесь телефоном, чтобы я мог идентифицировать вас.",
                    cancellationToken: cancellationToken
                )
            );
            
            await Task.Delay(1500);
            
            await _messageService.MarkMessageToDelete(
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text:
                    "Не переживайте! Ваши данные не передаются третьим лицам и хранятся на безопасном сервере.",
                    cancellationToken: cancellationToken
                )
            );

            await Task.Delay(1500);
            
            await _messageService.MarkMessageToDelete(
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id, text: "Чтобы отправить телефон нажмите на кнопку\n'Поделиться контактом'",
                    replyMarkup: Keyboard.RequestLocationAndContactKeyboard, cancellationToken: cancellationToken
                )
            );
            
            
        }
        
        public async Task SendUserFormToAdmin(
            ITelegramBotClient botClient,
            Message? message,
            UserModel user
        )
        {
            var getPhotoAsync = botClient.GetUserProfilePhotosAsync(message.Chat.Id);
           
           if (getPhotoAsync.Result.TotalCount > 0)
           {
               var photo = getPhotoAsync.Result.Photos[0];
               await _messageService.MarkMessageToDelete(
               await botClient.SendPhotoAsync(
                   AdminSettings.AdminChatId, photo[0].FileId,
                   $" Пользователь |{user.TelegramFirstName}|\n" + 
                   $" @{message.Chat.Username??"Нет публичного имени"}    |{user.UId}|\n" +
                   $" Моб.тел. |{user.Phone}|\n" +
                   $" Фамилия {user.LastName}, имя {user.FirstName}\n" +
                   $" Хочет авторизоваться в системе",
                   replyMarkup: Keyboard.ApproveDeclineKeyboard
               ));
           }
           else
           {
               var filePath = @"Files/BOT_NO_IMAGE.jpg";
               await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
               await _messageService.MarkMessageToDelete(
               await botClient.SendPhotoAsync(
                   AdminSettings.AdminChatId, fileStream,
                   $" Пользователь |{user.TelegramFirstName}|\n" +
                   $" @{message.Chat.Username??"Нет публичного имени"}    |{user.UId}|\n" +
                   $" Моб.тел. |{user.Phone}|\n" +
                   $" Фамилия {user.LastName}, имя {user.FirstName}\n" +
                   $" Хочет авторизоваться в системе",
                   replyMarkup: Keyboard.ApproveDeclineKeyboard));
                   fileStream.Close();
           }
        }
        
        private static async Task SendMainPicture(ITelegramBotClient botClient, Message? message)
        {  
            var filePath = @"Files/BOT_MAIN_WELCOME_PICTURE.png";
            await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            await botClient.SendPhotoAsync(chatId: message.Chat.Id, new InputOnlineFile(fileStream));
        }
                
    }
}