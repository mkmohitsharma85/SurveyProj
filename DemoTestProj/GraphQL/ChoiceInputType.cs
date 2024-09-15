using Survey.Contract.Model;

namespace DemoTestProj.GraphQL
{
    public class ChoiceInputType : InputObjectType<ChoiceDTO>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ChoiceDTO> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IntType>>(); // Required
            descriptor.Field(c => c.ChoiceText).Type<NonNullType<StringType>>(); // Required
            descriptor.Field(c => c.QuestionId).Type<NonNullType<IntType>>(); // Required
        }
    }
}
