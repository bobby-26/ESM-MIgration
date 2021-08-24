using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreArticles : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

          

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreArticles.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewArticelSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreArticlesSearch.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreArticles.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreArticlesAdd.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            Crewartical.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewArticelSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewarticelMenu_TabStripCommand(object sender, EventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = 1;
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDUPDATEON", "FLDUPDATEDONE", "FLDUPDATEDBY", "FLDMODIFIEDDATE" ,"FLDREMARKS" };
                string[] alCaptions = { "Updated Date", "Update Done", "Updated By", "Updated On", "Remarks" };

                NameValueCollection nvc = Filter.Currentcrewarticle;
                DataTable dt = PhoenixCrewOffshoreArticles.ArtcelsSearch(General.GetNullableInteger(nvc != null ? nvc["UcVessel"] : "")
               , General.GetNullableDateTime(nvc != null ? nvc["txtFromdate"] : null)
               , General.GetNullableDateTime(nvc != null ? nvc["txtTodate"] : null)
               , General.GetNullableInteger(nvc != null ? nvc["ucArticalType"] : "")
                                                       , General.GetNullableInteger(nvc != null ? nvc["chkInactive"] : "")
                                                       , sortexpression, sortdirection
                                                       , 1
                                                       , gvCrewArticelSearch.PageSize
                                                       , ref iRowCount
                                                       , ref iTotalPageCount
                                                       );
                General.ShowExcel("Crew Articles", dt, alColumns, alCaptions, 0, null);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.Currentcrewarticle = null;
                BindData();
                gvCrewArticelSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = null;
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = 1;
        try
        {
            string[] alColumns = { "FLDUPDATEON", "FLDUPDATEDONE", "FLDUPDATEDBY", "FLDMODIFIEDDATE", "FLDREMARKS" };
            string[] alCaptions = { "Updated Date", "Update Done", "Updated By", "Updated On", "Remarks" };

            NameValueCollection nvc = Filter.Currentcrewarticle;
            DataTable dt = PhoenixCrewOffshoreArticles.ArtcelsSearch(General.GetNullableInteger(nvc != null ? nvc["UcVessel"] : "")
                , General.GetNullableDateTime(nvc != null ? nvc["txtFromdate"]:null)
                , General.GetNullableDateTime(nvc != null ?nvc["txtTodate"]:null)
                , General.GetNullableInteger(nvc != null ? nvc["ucArticalType"]:"")
                                                        , General.GetNullableInteger(nvc != null ? nvc["chkInactive"] : "")
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , gvCrewArticelSearch.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewArticelSearch", "Crew Articles", alCaptions, alColumns, ds);
            gvCrewArticelSearch.DataSource = dt;
            gvCrewArticelSearch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
          
        }
         catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewArticelSearch.Rebind();
    }
   
    protected void gvCrewArticelSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewArticelSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewArticelSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
      
       
        try
        {
            if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                int articleid = int.Parse(((RadLabel)e.Item.FindControl("lblarticle")).Text);
                Response.Redirect("../CrewOffshore/CrewOffshoreArticlesAdd.aspx?articleid=" + articleid, true);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewArticelSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblarticle = (RadLabel)e.Item.FindControl("lblarticle");
            LinkButton imgaddedvessel = (LinkButton)e.Item.FindControl("imgaddedvessel");
            //HtmlImage  imgaddedvessel = (HtmlImage)e.Row.FindControl("imgaddedvessel");
            if (imgaddedvessel != null)
            {
                if (lblarticle.Text != "")
                {
                    imgaddedvessel.Visible = true;
                    imgaddedvessel.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreArticleVessel.aspx?articleid=" + lblarticle.Text + "')");
                }
                else
                {
                    imgaddedvessel.Visible = false;
                }
            }
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            HtmlGenericControl html = new HtmlGenericControl();
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                   
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.OFFSHORE + "'); return false;");
            }

            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip ucToolTipRemarks = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
            lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipRemarks.ToolTip + "', 'visible');");
            lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipRemarks.ToolTip + "', 'hidden');");

            RadTextBox txtremarkfilter = (RadTextBox)e.Item.FindControl("txtremarkfilter");
            if (txtremarkfilter != null) txtremarkfilter.Attributes.Add("OnTextChanged", "txtremarkfilter_TextChanged");
            UserControlDate txtupdatefilter = (UserControlDate)e.Item.FindControl("txtupdatefilter");
            if (txtupdatefilter != null) txtupdatefilter.Attributes.Add("OnTextChangedEvent", "txtupdatefilter_TextChangedEvent");

            if (e.Item is GridFilteringItem)
            {
                //RadTextBox txtremarkfilter = (RadTextBox)e.Item.FindControl("txtremarkfilter");
                //if (txtremarkfilter != null) txtremarkfilter.Attributes.Add("OnTextChanged", "txtremarkfilter_TextChanged");
                //UserControlDate txtupdatefilter = (UserControlDate)e.Item.FindControl("txtupdatefilter");
                //if (txtupdatefilter != null) txtupdatefilter.Attributes.Add("OnTextChangedEvent", "txtupdatefilter_TextChangedEvent");
            }

          
        }
    }

    protected void txtupdatefilter_TextChanged(object sender, EventArgs e)
    {
        NameValueCollection nvc = new NameValueCollection();
       
        nvc.Add("txtTodate", "01/01/2016");
     
        Filter.Currentcrewarticle = nvc;

        BindData();
        gvCrewArticelSearch.Rebind();

    }

    protected void txtremarkfilter_TextChanged(object sender, EventArgs e)
    {
        NameValueCollection nvc = new NameValueCollection();

        nvc.Add("txtTodate", "01/01/2016");

        Filter.Currentcrewarticle = nvc;

        BindData();
        gvCrewArticelSearch.Rebind();
    }

    protected void txtupdatefilter_TextChangedEvent(object sender, EventArgs e)
    {
        NameValueCollection nvc = new NameValueCollection();

        nvc.Add("txtTodate", "01/01/2016");

        Filter.Currentcrewarticle = nvc;

        BindData();
        gvCrewArticelSearch.Rebind();
    }
}
