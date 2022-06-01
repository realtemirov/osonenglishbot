using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using System.Linq;
using static dailywords_Robot.Program;
using dailywords_Robot;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace FIrstBotASP.Controllers
{
    public class HomeController : Controller
    {
        
        private TelegramBotClient client = new TelegramBotClient("YOUR_TOKEN");
        public static int counter = 0;
        
        // HomePage
        public string Index()
        {
            ++counter;
            if (counter == 1)
            {
                
                client.OnMessage += Xabar_Kelganda;
                
                client.StartReceiving();
            }

            // string qaytaradi  
            return "token";
        }

       
        // foydalanuvchu xabar yuborganda ishlaydi
        private async void Xabar_Kelganda(object sender, MessageEventArgs e)
        {
            long userId = e.Message.Chat.Id;
            int msgId = e.Message.MessageId;

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                
                string s = "";
                bool res = false, change = false;
                string k1 = "🇬🇧 - 5", k2 = "🇬🇧 - 10", k3 = "🇬🇧 - 15", k4 = "🇬🇧 - 20", k5 = "🇬🇧 => 🇺🇿";

                ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup();
                markup.ResizeKeyboard = true;
                markup.Keyboard = new KeyboardButton[][]
                    {
                    new KeyboardButton[]
                    {
                        new KeyboardButton(k1),
                        new KeyboardButton(k2)
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton(k3),
                        new KeyboardButton(k4)
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton(k5)
                    }
                    };

                string lang = "";

                try
                {
                    switch (e.Message.Text)
                    {
                        case "/start":
                            {
                                await client.SendTextMessageAsync(userId, "Assalomu alaykum", replyMarkup: markup, replyToMessageId: msgId);
                                User user = new User(e.Message.Chat.FirstName + e.Message.Chat.LastName, e.Message.Chat.Id, (e.Message.Date.Date + e.Message.Date.TimeOfDay).ToString());
                                Program.MyUsers(user);
                                lang = "en";
                                break;
                            }
                            
                        case "/count":
                            {
                                if (e.Message.Chat.Id == admin_id)
                                {
                                    await client.SendTextMessageAsync(userId, "count: " + Program.Count().ToString(), replyMarkup: markup, replyToMessageId: msgId);
                                    using (var stream = System.IO.File.OpenRead("users.json"))
                                    {
                                        await client.SendDocumentAsync(
                                            chatId: userId,
                                            document: stream,
                                            caption: "Your users",
                                            replyToMessageId: e.Message.MessageId,
                                            disableNotification: true
                                        );
                                    }
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(userId, e.Message.Text, replyMarkup: markup);
                                }
                                lang = "en";
                                break;
                            }

                        case "/statistics":
                            {
                                string stat = "";
                                string path = "wordseng.json";
                                var temp = System.IO.File.ReadAllText(path);
                                var readWords = JsonConvert.DeserializeObject<ListWords>(temp);
                                stat += $"🇬🇧 <b>English-Uzbek: <code>{readWords.words.Count}</code>\n</b>";

                                path = "wordsuzb.json";
                                temp = System.IO.File.ReadAllText(path);
                                readWords = JsonConvert.DeserializeObject<ListWords>(temp);
                                stat += $"🇺🇿 <b>Uzbek-English: <code>{readWords.words.Count}</code>\n</b>";

                                await client.SendTextMessageAsync(userId, stat, replyMarkup: markup, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                     replyToMessageId: msgId);
                                lang = "en";
                                break;
                            }

                        case "🇬🇧 => 🇺🇿":
                            {
                                change = true;
                                lang = "uz";
                                break;
                            }

                        case "🇺🇿 => 🇬🇧":
                            {
                                change = true;
                                lang = "en";
                                break;
                            }

                        case "🇬🇧 - 5":
                            {
                                s = Words(5, "🇬🇧"); res = true;
                                lang = "en";
                                break;
                            }
                        case "🇬🇧 - 10":
                            {
                                s = Words(10, "🇬🇧"); res = true;
                                lang = "en";
                                break;
                            }
                        case "🇬🇧 - 15":
                            {
                                s = Words(15, "🇬🇧"); res = true;
                                lang = "en";
                                break;
                            }
                        case "🇬🇧 - 20":
                            {
                                s = Words(20, "🇬🇧"); res = true;
                                lang = "en";
                                break;
                            }
                        case "🇺🇿 - 5":
                            {
                                s = Words(5, "🇺🇿"); res = true;
                                lang = "uz";
                                break;
                            }
                        case "🇺🇿 - 10":
                            {
                                s = Words(10, "🇺🇿"); res = true;
                                lang = "uz";
                                break;
                            }
                        case "🇺🇿 - 15":
                            {
                                s = Words(15, "🇺🇿"); res = true;
                                lang = "uz";
                                break;
                            }
                        case "🇺🇿 - 20":
                            {
                                s = Words(20, "🇺🇿"); res = true;
                                lang = "uz";
                                break;
                            }
                        default:
                            await client.SendTextMessageAsync(userId, e.Message.Text, replyMarkup: markup);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await client.SendTextMessageAsync(userId, "Uzr, test rejimida nimadir xato ketdi ! \nQaytadan tanglang: ", replyMarkup: markup);
                    await client.SendTextMessageAsync(265943548, ex.Message);
                }

                if (lang == "uz")
                {
                    k1 = "🇺🇿 - 5";
                    k2 = "🇺🇿 - 10";
                    k3 = "🇺🇿 - 15";
                    k4 = "🇺🇿 - 20";
                    k5 = "🇺🇿 => 🇬🇧";
                }
                else if (lang == "en")
                {
                    k1 = "🇬🇧 - 5";
                    k2 = "🇬🇧 - 10";
                    k3 = "🇬🇧 - 15";
                    k4 = "🇬🇧 - 20";
                    k5 = "🇬🇧 => 🇺🇿";
                }

                markup.Keyboard = new KeyboardButton[][]
                {
                new KeyboardButton[]
                {
                    new KeyboardButton(k1),
                    new KeyboardButton(k2)
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(k3),
                    new KeyboardButton(k4)
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(k5)
                }
                };

                //await client.SendTextMessageAsync(userId, e.Message.Text, replyMarkup: markup);
                if (res)
                    await client.SendTextMessageAsync(userId, s, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                        replyMarkup: markup, replyToMessageId: msgId);
                if (change)
                {
                    await client.SendTextMessageAsync(userId, "Change", replyMarkup: markup);
                }
                string forwardText = $"From: {e.Message.From.Id} => @{e.Message.From.Username} => \"{e.Message.From.FirstName}\" \n Text: {e.Message.Text} \nDay:{e.Message.Date.Date} {e.Message.Date.TimeOfDay}";
                await client.SendTextMessageAsync(265943548, forwardText);
            }
            else
            {
                await client.SendTextMessageAsync(userId, "Please select: ");
            }

        }

        public string Words(int count, string lang)
        {
            string allwords = "";
            string path = "";
            var rand = new Random();
            ListWords readWords = new ListWords();
            if (lang == "🇬🇧")
            {
                path = "wordseng.json";
                var temp = System.IO.File.ReadAllText(path);
                readWords = JsonConvert.DeserializeObject<ListWords>(temp);
                int n = 0;

                for (int i = 0; i < count; i++)
                {
                    n = rand.Next(0, 14425);
                    allwords += $"🇬🇧 <b>{readWords.words[n].eng}</b> \n🇺🇿 <i>{ readWords.words[n].uzb}</i>\n\n";
                }
            }
            else
            {
                path = "wordsuzb.json";
                var temp = System.IO.File.ReadAllText(path);
                readWords = JsonConvert.DeserializeObject<ListWords>(temp);
                int n = 0;
                for (int i = 0; i < count; i++)
                {
                    n = rand.Next(0, 26730);
                    allwords += $"🇺🇿 <b>{readWords.words[n].uzb}</b> \n🇬🇧 <i>{ readWords.words[n].eng}</i>\n\n";
                }
                
            }
            allwords += "🌐 <b>Kanal: @osonenglishuz </b>";
            return allwords;
        }

        public class Word
        {
            public string eng { get; set; }

            public string uzb { get; set; }

            public Word(string eng, string uzb)
            {
                this.eng = eng;
                this.uzb = uzb;
            }
        }

        public class ListWords
        {
            public List<Word> words { get; set; }

            public ListWords()
            {
                this.words = new List<Word>();
            }

        }

    }
}