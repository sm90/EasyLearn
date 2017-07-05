using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class MovieModel
    {
        public int ModuleId { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbUrl { get; set; }
        public List<QuestionModel> Questions { get; set; }

        public MovieModel()
        {
            Questions = new List<QuestionModel>();
        }
    }

    public class QuestionModel
    {
        public List<Alternative> Alternatives { get; set; }

        public QuestionModel()
        {
            Alternatives = new List<Alternative>();
        }
    }

    public class Alternative
    {
        public string ImgUrl { get; set; }
        public bool IsCorrect { get; set; }
    }
}