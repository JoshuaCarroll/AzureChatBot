    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class SelectDialog : IDialog<string>
    {
        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("How can I help you?");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var settings = new JsonSerializerSettings { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
            };
            string rtn = JsonConvert.SerializeObject(value, Formatting.Indented, settings);
            log.Info(rtn);

            await context.PostAsync("You said: " + message.Text);

            if ((message.Text.Contains("thanks")) || (message.Text.Contains("thank you")) && (message.Text.Contains("thx"))) {
              await context.PostAsync("Anytime. I'm happy to help.");
              context.Wait(this.MessageReceivedAsync);
            }
            else if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                /* Completes the dialog, removes it from the dialog stack, and returns the result to the parent/calling
                    dialog. */
                context.Done(message.Text);
            }
            /* Else, try again by re-prompting the user. */
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
                    /* Fails the current dialog, removes it from the dialog stack, and returns the exception to the
                        parent/calling dialog. */
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }
            }
        }
    }
