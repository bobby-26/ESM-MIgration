using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewTravelQuotationChatDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            HtmlMeta meta = new HtmlMeta();
            meta.HttpEquiv = "Refresh";
            Response.AppendHeader("Refresh", "60");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["QUOTEID"] != null)
                {
                    repDiscussion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                    ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
                }              
              
                bindChatData();              
            }                

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void bindChatData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuoteChatSearch(General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString())
                                , (int)ViewState["PAGENUMBER"],repDiscussion.PageSize, ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            repDiscussion.DataSource = ds.Tables[0];
            repDiscussion.VirtualItemCount = iRowCount;
        }
        else
        {
            repDiscussion.DataSource = "";
        }
    }

    protected void repDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : repDiscussion.CurrentPageIndex + 1;
        bindChatData();
    }

    protected void repDiscussion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
