    using System;
    using System.Threading.Tasks;

    public class Conversation
    {
        public List<Question> Questions;
        public Conversation() {
            Questions = new List<Question>();
        }
        public Conversation AddQuestion(string statement, string interrogative) {
            Question q = new Question(statement, interrogative);
            Questions.Add(q);
            return this;
        }
    }

    public class Question 
    {
        public string Statement;
        public string Interrogative;
        public List<Answer> Answers;

        public Question(string statement, string interrogative) {
            Statement = statement;
            Interrogative = interrogative;
        }

        public Question AddAnswer(AnswerType type, Func<String, String> callback) {
            Answer a = new Answer(type, callback);
            Answers.Add(a);

            return this;
        }
    }

    public class Answer
    {
        private Func<String, String> Callback;
        public AnswerType Type;
        public Answer(AnswerType type, Func<String, String> callback) {
            Type = type;
            Callback = callback;
        }
    }

    public enum AnswerType
    {
        Affirmative,
        Negative,
        Other
    }
