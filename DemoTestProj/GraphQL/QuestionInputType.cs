using Survey.Contract.Model;

namespace DemoTestProj.GraphQL
{
    public class QuestionInputType : InputObjectType<QuestionDTO>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QuestionDTO> descriptor)
        {
            descriptor.Field(q => q.Id).Type<NonNullType<IntType>>(); // Required
            descriptor.Field(q => q.SurveyId).Type<NonNullType<IntType>>(); // Required
            descriptor.Field(q => q.QuestionText).Type<NonNullType<StringType>>(); // Required
            descriptor.Field(q => q.QuestionType).Type<NonNullType<StringType>>(); // Required
            descriptor.Field(q => q.Choices).Type<ListType<ChoiceInputType>>(); // Optional
        }
    }
}
