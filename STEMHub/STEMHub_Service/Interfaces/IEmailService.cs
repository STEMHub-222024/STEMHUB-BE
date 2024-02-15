using STEMHub.STEMHub_Service.Constants;

namespace STEMHub.STEMHub_Service.Interfaces
{
    public interface IEmailService
    {
        public string SendEmail(Message message);
    }
}
