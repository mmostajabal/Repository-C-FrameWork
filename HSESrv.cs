using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameWorkSRV;
using HSEData;

namespace HSEServices
{
    public class HSESrv
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NationalId"></param>
        /// <param name="MobileNo"></param>
        /// <returns></returns>
        public static EmployeeMobileClass FetchEmployeeMobileTbl(string NationalId, string MobileNo)
        {
            EmployeeMobileClass employeeMobileClasses = null;
            var predicateEmployeeMobileTbl = PredicateBuilder.True<EmployeeMobileTbl>();
            IGenericRepository<EmployeeMobileTbl, EmployeeMobileClass> employeeMobileTblRepository = new ObjectClass.EmployeeMobileTblRepository();

            if (NationalId != "")
            {
                predicateEmployeeMobileTbl = predicateEmployeeMobileTbl.And(c => c.NationalId == NationalId);
            }

            if (MobileNo != "")
            {
                predicateEmployeeMobileTbl = predicateEmployeeMobileTbl.And(c => c.MobileNo == MobileNo);
            }

            employeeMobileClasses = employeeMobileTblRepository.FindBy(predicateEmployeeMobileTbl, true).Item2.FirstOrDefault();

            return employeeMobileClasses;
        }
        /// <summary>
        /// FetchTQuestion
        /// </summary>
        /// <returns></returns>
        public static IList<QuestionClass> FetchTQuestion()
        {
            IList<QuestionClass> questionClasses = null;

            var predicateTQuestion = PredicateBuilder.True<TQuestion>();
            IGenericRepository<TQuestion, QuestionClass> TQuestionRepository = new ObjectClass.TQuestionRepository();

            questionClasses = TQuestionRepository.FindBy(predicateTQuestion, true).Item2.ToList();

            return questionClasses.OrderBy(o =>o.ParentQuestionNo).ThenBy(o=>o.ChildQuestionNo).ToList();
        }
        /// <summary>
        /// FetchTAnswerItem
        /// </summary>
        /// <returns></returns>
        public static IList<AnswerItemClass> FetchTAnswerItem()
        {
            IList<AnswerItemClass> answerItemClasses = null;

            var predicateTAnswerItem = PredicateBuilder.True<TAnswerItem>();
            IGenericRepository<TAnswerItem, AnswerItemClass> TAnswerItemRepository = new ObjectClass.TAnswerItemRepository();

            answerItemClasses = TAnswerItemRepository.FindBy(predicateTAnswerItem, true).Item2.ToList();

            return answerItemClasses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hseQuestionId"></param>
        /// <param name="hseAnswerItemId"></param>
        /// <param name="curDate"></param>
        /// <param name="curTime"></param>
        /// <param name="entryDataNationalId"></param>
        /// <returns></returns>
        public static short SaveTTAnswer2Question(int[] hseQuestionId, int[] hseAnswerItemId, string curDate, string curTime, string entryDataNationalId)
        {
            IGenericRepository<TAnswer2Question, Answer2QuestionClass> TAnswerItemRepository = new ObjectClass.TAnswer2QuestionRepository();
            TAnswer2Question tAnswer2Question;
            int ind;
            short flag = 0;
            for (ind = 0; ind < hseQuestionId.Length; ind++)
            {
                tAnswer2Question = new TAnswer2Question();
                tAnswer2Question.HSE_AnswerItemId = hseAnswerItemId[ind];
                tAnswer2Question.HSE_QuestionId = hseQuestionId[ind];
                tAnswer2Question.EntryDataDate = curDate;
                tAnswer2Question.EntryDataTime = curTime;
                tAnswer2Question.EntryDataNationalId = entryDataNationalId;


                TAnswerItemRepository.Add(tAnswer2Question);
            }

            TAnswerItemRepository.Save();
            flag = 1;
            return flag;
        }
        /// <summary>
        /// FetchTAnswer2Question
        /// </summary>
        /// <param name="hseQuestionId"></param>
        /// <param name="hseAnswerItemId"></param>
        /// <param name="curDate"></param>
        /// <param name="entryDataNationalId"></param>
        /// <returns></returns>
        public static IList<Answer2QuestionClass> FetchTAnswer2Question(int hseQuestionId, int hseAnswerItemId, string curDate, string entryDataNationalId)
        {
            IList<Answer2QuestionClass> answer2QuestionClasses = null;
            var predicateTAnswer2Question = PredicateBuilder.True<TAnswer2Question>();
            IGenericRepository<TAnswer2Question, Answer2QuestionClass> TAnswerItemRepository = new ObjectClass.TAnswer2QuestionRepository();

            if (hseQuestionId != 0)
            {
                predicateTAnswer2Question = predicateTAnswer2Question.And(c => c.HSE_QuestionId == hseQuestionId);
            }

            if (hseAnswerItemId != 0)
            {
                predicateTAnswer2Question = predicateTAnswer2Question.And(c => c.HSE_AnswerItemId == hseAnswerItemId);
            }

            if (curDate != "")
            {
                predicateTAnswer2Question = predicateTAnswer2Question.And(c => c.EntryDataDate == curDate);
            }

            if (entryDataNationalId != "")
            {
                predicateTAnswer2Question = predicateTAnswer2Question.And(c => c.EntryDataNationalId == entryDataNationalId);
            }

            answer2QuestionClasses = TAnswerItemRepository.FindBy(predicateTAnswer2Question, true).Item2.ToList();

            return answer2QuestionClasses;
        }
    }
}
