namespace LiquorSales.Web.Pages
{
    public class ConfirmationModel : PageModel
    {
        public string Message { get; set; } = default!;

        public void OnGetContact()
        {
            Message = "Your e-mail was Sent!";
        }

        public void OnGetOrderSubmitted()
        {
            Message = "Your Order Was Submitted Successfully!";
        }
    }
}
