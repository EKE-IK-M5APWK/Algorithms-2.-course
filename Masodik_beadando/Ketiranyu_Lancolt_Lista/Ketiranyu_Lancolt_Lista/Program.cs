using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ketiranyu_Lancolt_Lista
{
	class Program
	{
		public class DoubleChain
		{
			public string Title { get; set; }
			public DoubleChain PreviousChain { get; set; }
			public DoubleChain NextChain { get; set; }

			public DoubleChain(string title)
			{
				Title = title;
			}

			public override string ToString()
			{
				return Title;
			}
		}
		public class DoubleChainedList
		{
			private DoubleChain first;
			public bool IsEmpty
			{
				get
				{
					return first == null;
				}
			}
			public DoubleChainedList()
			{
				first = null;
			}

			public DoubleChain Push(string title)
			{
				DoubleChain link = new DoubleChain(title);
				link.NextChain = first;
				if (first != null)
                {
					first.PreviousChain = link;
				}
				first = link;
				return link;
			}

			public DoubleChain Pop()
			{
				DoubleChain temp = first;
				if (first != null)
				{
					first = first.NextChain;
					if (first != null)
                    {
						first.PreviousChain = null;
					}					
				}
				return temp;
			}

			public override string ToString()
			{
				DoubleChain currentLink = first;
				StringBuilder builder = new StringBuilder();
				while (currentLink != null)
				{
					builder.Append(currentLink);
					currentLink = currentLink.NextChain;
				}
				return builder.ToString();
			}

		}
		public static void Main()
		{
			DoubleChainedList list = new DoubleChainedList();
			list.Push("30");
			list.Push("50");
			list.Push("70");
			list.Pop();
            Console.WriteLine(list.ToString());
			Console.ReadLine();
		}
	}
}
