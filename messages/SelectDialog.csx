    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class SelectDialog : IDialog<string>
    {
        private int attempts = 3;
        private string qID = "0";

        public async Task StartAsync(IDialogContext context)
        {
            string[] arrWelcome = { "How can I help?", "What can I do for you?", "Fire away.", "So what can I do for you?" };
            await context.PostAsync(arrWelcome[new Random().Next(0,arrWelcome.Length)]);

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if ((message.Text.Contains("disregard")) || (message.Text.Contains("quit")) || (message.Text.Contains("nevermind")) && (message.Text.Contains("never mind")))
            {
                /* Completes the dialog, removes it from the dialog stack, and returns the result to the parent/calling dialog. */
                await context.PostAsync("Ok then. Talk to you later.");
                context.Done(message.Text);
            }
            else {
                switch (qID) {
                    case "0":
                        if (message.Text.Contains("list") && message.Text.Contains("servers")) {
                            qID = "01";
                            await context.PostAsync("I can't list the servers yet. Will you be happy when I can?");
                        }
                        break;
                    case "01":
                        if (message.Text.Contains("yes")) {
                            await context.PostAsync("Me too. That'll be cool.");
                        }
                        else {
                            await context.PostAsync("Well who cares what you think.");
                        }
                        break;
                    case "999":
                        qID="00";
                        await context.PostAsync("So what else can I do for you today?");
                        break;
                    default:
                        await context.PostAsync(message.Name + " said, \"" + message.Text + "\"");
                        break;
                }
                context.Wait(this.MessageReceivedAsync);
            }

            
/* 
            if ((message.Text.Contains("thanks")) || (message.Text.Contains("thank you")) && (message.Text.Contains("thx"))) {
              await context.PostAsync("Anytime. I'm happy to help.");
              context.Wait(this.MessageReceivedAsync);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. Could you please try that again?");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }
            }
*/
        }
    }
