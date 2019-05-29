using System;
using System.Collections.Generic;

namespace Pos.BusinessLogic.Dto.Base
{
    public abstract class PagedResultBase : Result
    {
        public int CurrentPage { get; set; } 
        public int PageCount { get; set; } 
        public int PageSize { get; set; } 
        public int RowCount { get; set; }
 
        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;
        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }
 
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; } = new List<T>();

        public PagedResult<T2> CloneTo<T2>(IList<T2> newList) where T2 : class
        {
            return new PagedResult<T2>
            {
                CurrentPage = CurrentPage, 
                PageCount = PageCount, 
                PageSize = PageSize, 
                RowCount = RowCount,
                Results = newList,
                Messages =  Messages
            };;
        }
    }
}
