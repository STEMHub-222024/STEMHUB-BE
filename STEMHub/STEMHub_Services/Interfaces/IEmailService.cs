using STEMHub.STEMHub_Services.Constants;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface IEmailService
    {
        public string SendEmail(Message message);
    }
}
