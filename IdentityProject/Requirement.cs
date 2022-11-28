using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace IdentityProject
{
        public class ExpireDateExchangeRequirement : IAuthorizationRequirement
        {
          //Burayı boş bıraktık çünkü ExpireDateExchangeRequirement ismini startup da kısıtlama ismi olarak kullanacağız.

        }
            

        
        public class ExpireDateExchangeHandler : AuthorizationHandler<ExpireDateExchangeRequirement>    //
        {

            // Bu handle' ın görevi Kullanıcı Controllerı çalıştıran middleware ya gelmeden Authentication Authorization middlewarlerinde bizim değerimizin de kontrol edilmesini istiyoruz.
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExpireDateExchangeRequirement requirement)
            {
                if (context.User != null && context.User.Identity != null) //Öncelkle bu kullanıcı var mı ve üye mi ?
                {
                    var claim = context.User.Claims.Where(x => x.Type == "ExpireDateExchange" && x.Value != null).FirstOrDefault(); //Benim claimim var mı buna öncelikle bir bakıyoruz. Claimin tipi var mı ve null değil mi bunu bi kontrol edelim. Bu değerden tarihi karşılaştırıcaz.
                    if (claim != null)  // Eğer claim null değilse 
                    {
                        if (DateTime.Now < Convert.ToDateTime(claim.Value)) // Bizim value değerimizizi tarih formatında dönüştürüyoruz. ve bizim şu andaki güncel tarihten büyük ise kullanıcı girecek
                        {
                            context.Succeed(requirement);       //Context.Succedd (Authorization işleminin başarılı gerçekleştiğini belirtiyor.)
                        }
                        else
                        {
                            context.Fail();                     // Faile düştü
                        }
                    }
                }
                return Task.CompletedTask;          //Bir task dönmemiz gerekiyor görevin tamamlandıgını belirtmek için taski döndük.
            }


        }

    }
