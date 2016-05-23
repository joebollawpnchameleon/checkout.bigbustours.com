
using System.ComponentModel;

namespace Common.Enums
{
    public enum EmailTemplatePages
    {
        [Description("adminpages")]
        AdminPages,
        [Description("agent_register.aspx")]
        AgentRegister,
        [Description("all")]
        All,
        [Description("enquiry_form.aspx")]
        EnquiryForm,
        [Description("eVoucher")]
        EVoucher,
        [Description("eVoucherCopyEmail")]
        EVoucherCopyEmail,
        [Description("groupbooking")]
        GroupBooking,
        [Description("london")]
        London,
        [Description("worlpayaccounts")]
        WorldPayAccounts
    }
}
