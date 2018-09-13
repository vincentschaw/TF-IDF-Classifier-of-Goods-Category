using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject
{
    class TypeModel
    {
        public string maleSuit, maleWinterClothes, maleSweater, maleJacket, maleSportswear, maleShirt, malePants, maleUnderwear, femaleCoat, femaleWinterClothes, femaleSweater, femaleSportswear, femaleShirt, femaleDress, femalePants, femaleUnderwear;

        static public string[] GetTypeNameStringArray()
        {
            string[] result = new string[] { "maleSweater","maleSuit","maleWinterClothes", "maleJacket", "maleSportswear", "maleShirt", "malePants", "maleUnderwear", "femaleCoat", "femaleWinterClothes", "femaleSweater", "femaleSportswear", "femaleShirt", "femaleDress", "femalePants", "femaleUnderwear" };
            return result;
        }
    }
}
