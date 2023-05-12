using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Services.Models.Enums
{
    public enum UserMessagesKeys
    {
        /// <summary>
        /// Added Successfully
        /// </summary>
        AddedSuccessfully,

        /// <summary>
        /// Entity Not Found
        /// </summary>
        EntityNotFound,

        /// <summary>
        /// Cannot Delete This Customer
        /// </summary>
        CannotDeleteThisCustomer,

        /// <summary>
        /// Customer has Active Accounts
        /// </summary>
        CustomerHasActiveAccounts,

        /// <summary>
        /// Updated Successfully
        /// </summary>
        UpdatedSuccessfully,

        /// <summary>
        /// Deleted Successfully
        /// </summary>
        DeletedSuccessfully,

        /// <summary>
        /// Email Already used!
        /// </summary>
        EmailAlreadyUsed,

        /// <summary>
        /// Phone Number Already used!
        /// </summary>
        PhoneNumberAlreadyUsed
    }
}
