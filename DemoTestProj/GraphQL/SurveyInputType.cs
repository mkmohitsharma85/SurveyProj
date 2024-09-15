using Survey.Contract.Model;

namespace DemoTestProj.GraphQL
{
    public class SurveyInputType : InputObjectType<SurveyDTO>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SurveyDTO> descriptor)
        {
            descriptor.Field(s => s.Title).Type<NonNullType<StringType>>(); // Required
            descriptor.Field(s => s.Description).Type<StringType>(); // Optional
            descriptor.Field(s => s.CreatedDate).Type<NonNullType<DateTimeType>>(); // Required
            descriptor.Field(s => s.ExpiryDate).Type<NonNullType<DateTimeType>>(); // Required
            descriptor.Field(s => s.Questions).Type<ListType<QuestionInputType>>(); // Optional
        }
    }
}
