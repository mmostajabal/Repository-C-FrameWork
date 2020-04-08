using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSEServices
{
    public class HSEClass
    {
    }

    public class Login
    {
        public string NationalId { get; set; }
        public string MobileNo { get; set; }
    }

    public class EmployeeMobileClass
    {
        public int hse_EmployeeMobileId { get; set; }
        public long PersonalId { get; set; }
        public string CartNo { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string MobileNo { get; set; }
        public short ComeFromWhichTable { get; set; }
        public string GenderDesc { get; set; }
        public int Gender { get; set; }
        public short flagUpdated { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedTime { get; set; }
        public string OldMobile { get; set; }
        public string NationalId { get; set; }
    }

    public class Answer2QuestionClass
    {
        public int HSE_Answer2QuestionId { get; set; }
        public int HSE_QuestionId { get; set; }
        public int HSE_AnswerItemId { get; set; }
        public string AnswerDesc { get; set; }
    }

    public class AnswerItemClass
    {
        public int HSE_AnswerItemId { get; set; }
        public string AnswerItemDesc { get; set; }
        public short IsActive { get; set; }

        public int HSE_SetAnswerId { get; set; }
    }

    public class QuestionClass
    {
        public int HSE_QuestionId { get; set; }
        public string QuestionDesc { get; set; }
        public short IsActive { get; set; }

        public int HSE_SetAnswerId { get; set; }

        public int QuestionNo { get; set; }

        public int HSE_ParentQuestionId { get; set; }
        public int HSE_ParentAnswerItemId { get; set; }
        public int ChildQuestionNo { get; set; }
        public int ParentQuestionNo { get; set; }

    }
    public class SetAnswerClass
    {
        public int HSE_SetAnswerId { get; set; }
        public string SetAnswerDesc { get; set; }
        public short IsActive { get; set; }
    }


}
