#load "AgeDialog.csx"
#load "SelectDialog.csx"

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

[Serializable]
public class RootDialog : IDialog<object>
{

  private string name;
  private int age;

  public async Task StartAsync(IDialogContext context)
  {
    context.Wait(this.MessageReceivedAsync);
  }

  private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
  {
    var message = await result;

    await this.SendWelcomeMessageAsync(context);
  }

  private async Task SendWelcomeMessageAsync(IDialogContext context)
  {
    context.Call(new SelectDialog(), this.SelectDialogResumeAfter);
  }

  private async Task SelectDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
  {
    try
    {
      this.name = await result;

      //context.Call(new AgeDialog(this.name), this.AgeDialogResumeAfter);
    }
    catch (TooManyAttemptsException)
    {
      await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

      await this.SendWelcomeMessageAsync(context);
    }
  }
}
