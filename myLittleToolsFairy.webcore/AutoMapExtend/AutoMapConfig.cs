using AutoMapper;
using myLittleToolsFairy.DbModels.Models;
using myLittleToolsFairy.IBusinessServices.Model;
using myLittleToolsFairy.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.WebCore.AutoMapExtend
{
    public class AutoMapConfig : Profile
    {
        public AutoMapConfig()
        {
            // 不需替 Entity 與 Dto 間名稱完全相同的屬性作設定
            CreateMap<UserEntity, UserDto>()
                // 屬性名稱不同
                .ForMember(c => c.UserName, s => s.MapFrom(x => x.Name))
                // Dto有但Entity中沒有的屬性
                .ForMember(c => c.IgnoreAtb, s => s.Ignore())
                // 雙向映射
                .ReverseMap();

            // PagingData 泛型映射
            CreateMap(typeof(PagingData<>), typeof(PagingData<>));
        }
    }
}