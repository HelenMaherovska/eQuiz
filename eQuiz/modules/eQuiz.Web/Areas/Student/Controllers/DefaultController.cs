﻿using eQuiz.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eQuiz.Repositories.Abstract;
using eQuiz.Repositories.Concrete;
using eQuiz.Entities;
using Newtonsoft.Json;
using eQuiz.Web.Areas.Student.Models;

namespace eQuiz.Web.Areas.Student.Controllers
{
    public class DefaultController : BaseController
    {
        #region Private Members

        private readonly IRepository _repository;

        #endregion

        #region Constructors

        public DefaultController(IRepository repository)
        {
            this._repository = repository;
        }
        #endregion

        #region Action Methods

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllQuizzes()
        {
            IEnumerable<Quiz> allQuizzes = _repository.Get<Quiz>();

            var result = from q in allQuizzes
                         select new
                         {
                             Id = q.Id,
                             Name = q.Name,
                             StartDate = q.StartDate.HasValue ? q.StartDate.Value.ToString() : "No start date",
                             TimeLimitMinutes = q.TimeLimitMinutes,
                             InternetAccess = q.InternetAccess
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuestionsByQuizId(int id)
        {
            var listQuizes = _repository.Get<Quiz>();
            var listQuestions = _repository.Get<Question>();
            var listQuestionTypes = _repository.Get<QuestionType>();
            var listQuizQuestions = _repository.Get<QuizQuestion>();
            var listQuizVariants = _repository.Get<QuizVariant>();
            var listQuestionAnswers = _repository.Get<QuestionAnswer>();
            var listAnswers = _repository.Get<Answer>();

            var quizInfo = from quiz in listQuizes
                           join variant in listQuizVariants on quiz.Id equals variant.QuizId
                           join quizQuestion in listQuizQuestions on variant.Id equals quizQuestion.QuizVariantId
                           join question in listQuestions on quizQuestion.QuestionId equals question.Id
                           join questionType in listQuestionTypes on question.QuestionTypeId equals questionType.Id

                           where quiz.Id == id
                           select new
                           {
                               Id = question.Id,
                               Text = question.QuestionText,
                               IsAutomatic = questionType.IsAutomatic,
                               Answers = from questAnswer in listQuestionAnswers
                                         join answer in listAnswers on questAnswer.AnswerId equals answer.Id
                                         where questAnswer.QuestionId == question.Id
                                         select new
                                         {
                                             Text = answer.AnswerText,
                                             Id = answer.Id
                                         },
                               QuizBlock = quizQuestion.QuizBlockId,
                               QuestionOrder = quizQuestion.QuestionOrder
                           };
            var quizInfoList = quizInfo.OrderBy(q => q.QuestionOrder).ToList();

            return Json(quizInfoList, JsonRequestBehavior.AllowGet);

        }


        public void InsertQuizResult(QuizResultModel passedQuiz)
        {
            QuizPass quizPassToInsert = new QuizPass
            {
                QuizId = passedQuiz.QuizId,
                UserId = 1,
                StartTime = passedQuiz.StartDate,
                FinishTime = passedQuiz.FinishDate
            };
            _repository.Insert<QuizPass>(quizPassToInsert);

            var lastQuizPassIdentity = quizPassToInsert.Id;

            foreach(var userAnswer in passedQuiz.UserAnswers)
            {
                if(userAnswer != null)
                {
                    QuizPassQuestion quizPassQuestionToInsert = new QuizPassQuestion
                    {
                        QuizPassId = lastQuizPassIdentity,
                        QuestionId = userAnswer.QuestionId,
                        QuizBlockId = userAnswer.QuizBlock,
                        //Remade GetQuestionsByQuizId method to send questionOrder property on the client
                        QuestionOrder = userAnswer.QuestionOrder
                    };

                    _repository.Insert<QuizPassQuestion>(quizPassQuestionToInsert);

                    var lastQuizPassQuestionIdentity = quizPassQuestionToInsert.Id;

                    if (userAnswer.IsAutomatic)
                    {
                        UserAnswer userAnswerToInsert = new UserAnswer
                        {
                            QuizPassQuestionId = lastQuizPassQuestionIdentity,
                            AnswerId = (int)userAnswer.AnswerId,
                            AnswerTime = userAnswer.AnswerTime
                        };

                        _repository.Insert<UserAnswer>(userAnswerToInsert);
                    }
                    else
                    {
                        UserTextAnswer userTextAnswerToInsert = new UserTextAnswer
                        {
                            QuizPassQuestionId = lastQuizPassQuestionIdentity,
                            AnswerText = userAnswer.AnswerText,
                            AnswerTime = userAnswer.AnswerTime
                        };

                        _repository.Insert<UserTextAnswer>(userTextAnswerToInsert);
                    }
                }
            }
        }
        #endregion
    }
}   