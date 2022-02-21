using System.Runtime.Serialization;

namespace Grade.Models.Dto
{
    public enum ESectionDtoDetailsType
    {
        [EnumMember(Value = nameof(Weekly))]
        Weekly, 
        [EnumMember(Value = nameof(Loose))]
        Loose
    }
}
