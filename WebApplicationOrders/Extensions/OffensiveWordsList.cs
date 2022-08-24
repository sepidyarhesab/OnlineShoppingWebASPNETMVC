using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepidyarHesabExtensions.Classes
{
    public static class OffensiveWordsList
    {

        public static List<string> GetOffensiveWorldsList = new List<string>();
        public static string GetOffensiveWorld = "fuck,mother,father,beach,Kir,kos,kon,kion,kharkosde,madarjende,kose madar,ghoromsagh,pofioz,kiir,kiiir,kiiiir,gaedamet,migayamet,madar jende,tokhmi,tokhm,khayemal,khaye,nane jende,siktir,sik,siiiktiiir,siiktiir,sag,kionde,koonde,kunde,goh,antar,oskol,osgol,pedar sag,sag nane,kir to kon,binamoos,binamus,koskhol,kiiir,kooos,harumzade,haroomzade,harumi,zaneto,kardani,kooni,kioni,bache kuni,koon,kun";
        public static void GetOffensiveWorlds()
        {
            var collection =
                "کیر,کون,کس,خواهر,مادر,کسکش,کصکش,کص,کیون,جنده,کونی,لاشی,دیوث,پفیوز,غرمصاق,حرومزاده,حرامزاده,کیری,تخم,تخمی,سگ,کسده,کسته,کصده,کونکش,پتیاره,ناموس,بیناموس,بیناموسی,گمشو,کیرمو,کونت,گه,گوه,انتر,انتری,عن,عنی,سیکتیر,کصخل,کسخل,کسخول,کصخول,اوسکول,اسکول,اسگول,اسکولی,کییر,کیییر,کییییر,کیییییر,کیییییییر,کییییییییر,کیونده,قرمساق,عنتر,حرومی,مادرجنده,خارمادر,کس‌مادر,کسه‌ننه,گاییدم,میگام,گاییدمت,کونپاره,خارکسده,کسپاره,کوص,ننه‌جنده,عنتر,عن‌اقا,کوسی,کوصی,خایمال,خایه,بچه‌کونی,كير,كون,كس,خواهر,مادر,كسكش,كصكش,كص,كيون,جنده,كونی,لاشي,ديوث,پفيوز,غرمصاق,حرومزاده,حرامزاده,كيري,تخم,تخمي,سگ,كسده,كسته,كصده,كونكش,پتياره,ناموس,بيناموس,بيناموسی,گمشو,كيرمو,كونت,گه,گوه,انتر,انتري,عن,عني,سيكتير,كصخل,كسخل,كسخول,كصخول,اوسكول,اسكول,اسگول,اسكولی,كيير,كييير,كيييير,كييييير,كیییییییر,كيييييير,كيونده,قرمساق,عنتر,حرومي,مادرجنده,خارمادر,کس‌مادر,کس‌ننه,گاييدم,ميگام,گاييدمت,كونپاره,خاركسده,كسپاره,كوص,ننه‌جنده,عنتر,عن‌اقا,كوسي,کیری,كوصي,خايمال,خايه,بچه‌كوني,Kir,kos,kon,kion,kharkosde,madarjende,kose madar,ghoromsagh,pofioz,kiir,kiiir,kiiiir,gaedamet,migayamet,madar jende,tokhmi,tokhm,khayemal,khaye,nane jende,siktir,sik,siiiktiiir,siiktiir,sag,kionde,koonde,kunde,goh,antar,oskol,osgol,pedar sag,sag nane,kir to kon,binamoos,binamus,koskhol,kiiir,kooos,harumzade,haroomzade,harumi,zaneto,kardani,kooni,kioni,bache kuni,koon,kun,کیر,کس,جون,کس ننت,کیری,کونت,کونی,کسده,کیرم,کست";
            foreach (var VARIABLE in collection.Split(','))
            { 
                GetOffensiveWorldsList.Add(VARIABLE);
            }
        }
    }
}
