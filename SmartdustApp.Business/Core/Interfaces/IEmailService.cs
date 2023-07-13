

using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace TestingAndCalibrationLabs.Business.Core.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Implimenting interface for Sendmail Method
        /// </summary>
        /// <param name="surveyModel"></param>
        /// <returns></returns>
        RequestResult<bool> Sendemail(EmailModel emailModel);
    }
}