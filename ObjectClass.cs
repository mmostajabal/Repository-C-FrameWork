using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameWorkSRV;
using HSEData;

namespace HSEServices
{
    public class ObjectClass
    {
        //  EmployeeMobileTbl
        public interface IEmployeeMobileTblRepository : IGenericRepository<EmployeeMobileTbl, EmployeeMobileClass>
        {
            //IQueryable GetJobAndInjuryTbl();
        }

        public class EmployeeMobileTblRepository : GenericRepository<HSEDBEntities, EmployeeMobileTbl, EmployeeMobileClass>, IEmployeeMobileTblRepository
        {
        }
        //  TQuestion
        public interface ITQuestionRepository : IGenericRepository<TQuestion, QuestionClass>
        {
            //IQueryable GetJobAndInjuryTbl();
        }

        public class TQuestionRepository : GenericRepository<HSEDBEntities, TQuestion, QuestionClass>, ITQuestionRepository
        {
        }

        //  TAnswerItem
        public interface ITAnswerItemRepository : IGenericRepository<TAnswerItem, AnswerItemClass>
        {
            //IQueryable GetJobAndInjuryTbl();
        }

        public class TAnswerItemRepository : GenericRepository<HSEDBEntities, TAnswerItem, AnswerItemClass>, ITAnswerItemRepository
        {
        }
        //  TSetAnswer
        public interface ITSetAnswerRepository : IGenericRepository<TSetAnswer, SetAnswerClass>
        {
            //IQueryable GetJobAndInjuryTbl();
        }

        public class TSetAnswerRepository : GenericRepository<HSEDBEntities, TSetAnswer, SetAnswerClass>, ITSetAnswerRepository
        {
        }
        //  TAnswer2Question
        public interface ITAnswer2QuestionRepository : IGenericRepository<TAnswer2Question, Answer2QuestionClass>
        {
            //IQueryable GetJobAndInjuryTbl();
        }

        public class TAnswer2QuestionRepository : GenericRepository<HSEDBEntities, TAnswer2Question, Answer2QuestionClass>, ITAnswer2QuestionRepository
        {
        }


    }
}
