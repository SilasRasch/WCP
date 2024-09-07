using WCPShared.Services;

SlackMessagingService slack = new SlackMessagingService();

Console.WriteLine("Sending message...");
bool res = await slack.SendMessageToUser("Silas H. Rasch", "Hej idiot");
Console.WriteLine("Message sent!");
Console.WriteLine(res);