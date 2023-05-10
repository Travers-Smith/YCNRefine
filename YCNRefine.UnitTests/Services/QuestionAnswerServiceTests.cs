using Moq;
using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Services;

namespace YCNRefine.UnitTests.Services
{
    public class QuestionAnswerServiceTests
    {
        [Fact] 
        public async Task Add_Successful_AddsQuestionAnswer()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            QuestionAnswer questionAnswer = new();

            unitOfWork.Setup(uow => uow.QuestionAnswer.AddAsync(questionAnswer));

            await new QuestionAnswerService(unitOfWork.Object).Add(questionAnswer);

            unitOfWork.Verify(uow => uow.QuestionAnswer.AddAsync(questionAnswer), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);    
        }

        [Fact]
        public async Task Add_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            QuestionAnswer questionAnswer = new();

            unitOfWork
                .Setup(uow => uow.QuestionAnswer.AddAsync(questionAnswer))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new QuestionAnswerService(unitOfWork.Object).Add(questionAnswer));

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task GetCorrectByOriginalSourceId_Success_ReturnsIEnumerableQuestionsAnswers()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            int originalSourceId = 1;
            int skip = 10;
            int take = 10;

            QuestionAnswer[] questionAnswers = new QuestionAnswer[]
            {
                new QuestionAnswer()
            };

            unitOfWork
                .Setup(uow => uow.QuestionAnswer.GetCorrectByOriginalSourceId(originalSourceId, skip, take))
                .ReturnsAsync(questionAnswers);

            Assert.Equal(questionAnswers, await new QuestionAnswerService(unitOfWork.Object).GetCorrectByOriginalSourceId(originalSourceId, skip, take));
        }

        [Fact]
        public async Task GetCorrectByOriginalSourceId_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            int originalSourceId = 1;
            int skip = 10;
            int take = 10;

            QuestionAnswer[] questionAnswers = new QuestionAnswer[]
            {
                new QuestionAnswer()
            };

            unitOfWork
                .Setup(uow => uow.QuestionAnswer.GetCorrectByOriginalSourceId(originalSourceId, skip, take))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new QuestionAnswerService(unitOfWork.Object).GetCorrectByOriginalSourceId(originalSourceId, skip, take));
        }
        [Fact]
        public async Task Update_Successful_UpdatesQuestionAnswer()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            QuestionAnswer questionAnswer = new();

            unitOfWork.Setup(uow => uow.QuestionAnswer.Update(questionAnswer));

            await new QuestionAnswerService(unitOfWork.Object).Update(questionAnswer);

            unitOfWork.Verify(uow => uow.QuestionAnswer.Update(questionAnswer), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            QuestionAnswer questionAnswer = new();

            unitOfWork
                .Setup(uow => uow.QuestionAnswer.Update(questionAnswer))
                .Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new QuestionAnswerService(unitOfWork.Object).Update(questionAnswer));

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}
