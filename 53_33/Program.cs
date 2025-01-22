using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _53_33
{
    internal class Program
    {
        //不同客戶名稱 (1)
        public enum CustomerType
        {
            Standard,
            SmallAndMediumBusiness,
            Enterprise,
            Government
        }
        static void Main(string[] args)
        {
            string input;
            decimal principal;
            do
            {
                Console.WriteLine("請輸入本金："); //輸入本金 (2)
                input = Console.ReadLine();
                //判斷書入文字是否能轉成 decimal，並做轉換
                if (decimal.TryParse(input, out principal))
                {
                    Console.WriteLine($"您輸入的本金是 ${principal:0,0.00}\n");
                    break;
                }
                else
                {
                    Console.WriteLine("金額輸入錯誤! 請重新輸入。");
                }
            } while (true);
            // 計算貸款本利和
            Console.WriteLine("==== LOAN ====");
            //將 CustomerType 內，每個元件叫出來運算
            foreach (CustomerType customerType in Enum.GetValues(typeof(CustomerType)))
            {
                RateCalculator loanCalculator = new LoanRateCalculator();
                try
                {
                    Console.WriteLine($"{loanCalculator.Calculate(customerType, principal):C}");
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine($"錯誤：{ex.Message}");
                }
            }
            // 計算存款本利和
            Console.WriteLine("==== SAVING ====");
            foreach (CustomerType customerType in Enum.GetValues(typeof(CustomerType)))
            {
                RateCalculator savingCalculator = new SavingRateCalculator();
                try
                {
                    Console.WriteLine($"{savingCalculator.Calculate(customerType, principal):C}");
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine($"錯誤：{ex.Message}");
                }
            }
            Console.ReadLine();
        }

        //原計算本利和程式 RateCalculator
        public abstract class RateCalculator
        {
            public abstract decimal Calculate(CustomerType type, decimal amount);
        }

        //繼承原本利和計算的貸款程式 LoanRateCalculator
        //LoanRateCalculator 物件繼承 RateCalculator
        public class LoanRateCalculator : RateCalculator
        {
            //複寫 RateCalculator 物件的 Calculate 方法 (4)
            public override decimal Calculate(CustomerType type, decimal amount)
            {
                decimal loanRate;
                switch (type)
                {
                    case CustomerType.Standard:
                        loanRate = 0.09m;
                        break;
                    case CustomerType.SmallAndMediumBusiness:
                        loanRate = 0.07m;
                        break;
                    case CustomerType.Enterprise:
                        loanRate = 0.06m;
                        break;
                    case CustomerType.Government:
                        loanRate = 0.03m;
                        break;
                    default:
                        //當在 switch 陳述式中找到不支援的 CustomerType 時，
                        //拋出一個 NotSupportedException 例外，
					//並且附帶一個描述信息為 "Unsupported CustomerType"。
					throw new NotSupportedException("Unsupported CustomerType");
                }
                return amount * (1 + loanRate); //傳回單利本利和 (4)
            }
        }
        //SavingRateCalculator 物件繼承 RateCalculator
        public class SavingRateCalculator : RateCalculator
        {
            //複寫 RateCalculator 物件的 Calculate 方法
            public override decimal Calculate(CustomerType type, decimal amount)
            {
                decimal savingRate;
                switch (type)
                {
                    case CustomerType.Standard:
                        savingRate = 0.06m;
                        break;
                    case CustomerType.SmallAndMediumBusiness:
                        savingRate = 0.035m;
                        break;
                    case CustomerType.Enterprise:
                        savingRate = 0.03m;
                        break;
                    case CustomerType.Government:
                        savingRate = 0.02m;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported CustomerType");
                }
                return amount * (1 + savingRate);
            }
        }
    }
}
