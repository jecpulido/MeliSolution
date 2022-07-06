using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Meli.Common.Enum
{
    public enum ValidationType
    {
        Request,
        Symmetrical,
        Characters,
        Mutant
    }

    public class Validation {

        public static int GetStatudCode(ValidationType type, bool IsSuccess) {

            switch (type)
            {
                case ValidationType.Request:                    
                case ValidationType.Symmetrical:
                case ValidationType.Characters:
                    return (int)HttpStatusCode.BadRequest;
                case ValidationType.Mutant:
                    if (IsSuccess)
                        return (int)HttpStatusCode.OK;
                    else
                        return (int)HttpStatusCode.Forbidden;
                default:
                    return (int)HttpStatusCode.OK;
            }

        }
    
    }
}
