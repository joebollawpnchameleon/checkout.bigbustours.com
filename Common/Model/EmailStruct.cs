
namespace Common.Model
{
    public class EmailStruct
    {
        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverEmail { get; set; }

        public string EmailBody { get; set; }

        public string EmailTitle { get; set; }

        public string AttachmentPath { get; set; }

        public string AttachmentName { get; set; }
    }

    public class HtmlEmailStruct : EmailStruct
    {
        public string HTMLBody { get; set; }
    }
}