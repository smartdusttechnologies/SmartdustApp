using System.Collections.Generic;

namespace SmartdustApp.Business.Common
{
    /// <summary>
    /// This is the base cass of RequestResults.
    /// </summary>
    public class ResultBase
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the validation messages.
        /// </summary>
        /// <value>The validation messages.</value>
        public IList<ValidationMessage> Message { get; set; }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Determines whether this instance has any failure Validation Messages
        /// </summary>
        protected bool HasFailureMessages()
        {
            return this.HasFailureValidationMessages();
        }

        #endregion


    }
}
