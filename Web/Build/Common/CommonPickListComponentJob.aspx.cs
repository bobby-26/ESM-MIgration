using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonPickListComponentJob : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (Request.QueryString["mode"] == "multi")
            toolbarmain.AddButton("Save", "ADD",ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuComponentType.MenuList = toolbarmain.Show();

		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
            gvComponentJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            btnConfirm.Attributes.Add("style", "display:none");
        }		
	}

	protected void MenuComponentType_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
			{
                ViewState["PAGENUMBER"] = 1;
				BindData();
                gvComponentJob.Rebind();			
			}
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                NameValueCollection nvc = new NameValueCollection();

                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
                string strNumber = string.Empty;
                string strName = string.Empty;
                string strId = string.Empty;

                foreach (GridDataItem gr in gvComponentJob.Items)
                {
                    if (((RadCheckBox)gr.FindControl("chkSelect")).Checked == true)
                    {
                        strNumber += ((RadLabel)gr.FindControl("lblJobCode")).Text + ",";
                        strName += ((LinkButton)gr.FindControl("lnkJobName")).Text + ",";
                        strId += ((RadLabel)gr.FindControl("lblComponentJobId")).Text + ",";
                    }
                }
                if (strId == string.Empty)
                {
                    ucError.ErrorMessage = "Select atleast one or more Component Jobs";
                    ucError.Visible = true;
                    return;
                }
                nvc.Add("lblJobCode", strNumber.TrimEnd(','));
                nvc.Add("lnkJobName", strName.TrimEnd(','));
                nvc.Add("lblComponentJobId", strId.TrimEnd(','));
                Filter.CurrentPickListSelection = nvc;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
 

	private void BindData()
	{

		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			DataSet ds;

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCommonPlannedMaintenance.RoundsComponentJobSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
                      , txtNumber.Text, txtName.Text,txtjobcode.Text,txtjobtitle.Text, sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvComponentJob.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);

            gvComponentJob.DataSource = ds;
            gvComponentJob.VirtualItemCount = iRowCount;

			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        int iMessageCode = 0;
        string iMessageText = "";
        PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), ViewState["COMPONENTJOBID"].ToString(), null, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);
        string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
        Script += "</script>" + "\n";
        NameValueCollection nvc = new NameValueCollection();
        
        nvc.Add("lblJobCode", string.Empty);        
        nvc.Add("lnkJobName", string.Empty);
        nvc.Add("lblComponentJobId", ViewState["COMPONENTJOBID"].ToString());
        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvComponentJob_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponentJob.CurrentPageIndex + 1;
        BindData();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            RadCheckBox obj = sender as RadCheckBox;
            GridDataItem gvr = (GridDataItem)obj.Parent.Parent;
            string componentjobid = ((RadLabel)gvr.FindControl("lblComponentJobId")).Text;
            ViewState["COMPONENTJOBID"] = componentjobid;
            int iMessageCode = 0;
            string iMessageText = "";
            if (obj.Checked == true)
                PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), componentjobid, null, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);
            else
                PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), null, componentjobid, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);

            if (iMessageCode == 1)
                //throw new ApplicationException(iMessageText);
                RadWindowManager1.RadConfirm(iMessageText, "confirm", 320, 150, null, "Confirm");

            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
            Script += "</script>" + "\n";
            NameValueCollection nvc = new NameValueCollection();

            RadLabel lblComponentNumber = (RadLabel)gvr.FindControl("lblJobCode");
            nvc.Add("lblJobCode", lblComponentNumber.Text);
            LinkButton lnkJob = (LinkButton)gvr.FindControl("lnkJobName");
            nvc.Add("lnkJobName", lnkJob.Text);
            nvc.Add("lblComponentJobId", componentjobid);
            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        }
        catch (ApplicationException aex)
        {
            ucConfirm.HeaderMessage = "Please Confirm";
            ucConfirm.ErrorMessage = aex.Message;
            ucConfirm.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName == "Select")
            {
                if (Request.QueryString["mode"] == "custom" || Request.QueryString["mode"] == "multi")
                {

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = new NameValueCollection();

                    RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblJobCode");
                    nvc.Add(lblComponentNumber.ID, lblComponentNumber.Text);
                    LinkButton lbComponentTypeName = (LinkButton)e.Item.FindControl("lnkJobName");
                    nvc.Add(lbComponentTypeName.ID, lbComponentTypeName.Text);
                    RadLabel lblComponentTypeID = (RadLabel)e.Item.FindControl("lblComponentJobId");
                    nvc.Add(lblComponentTypeID.ID, lblComponentTypeID.Text);
                }
                else
                {

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = Filter.CurrentPickListSelection;

                    RadLabel lblComponentNumber = (RadLabel)e.Item.FindControl("lblJobCode");
                    nvc.Set(nvc.GetKey(1), lblComponentNumber.Text.ToString());
                    LinkButton lbComponentTypeName = (LinkButton)e.Item.FindControl("lnkJobName");
                    nvc.Set(nvc.GetKey(2), lbComponentTypeName.Text.ToString());
                    RadLabel lblComponentTypeID = (RadLabel)e.Item.FindControl("lblComponentJobId");
                    nvc.Set(nvc.GetKey(3), lblComponentTypeID.Text);

                }
                Filter.CurrentPickListSelection = nvc;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvComponentJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton lnk = (LinkButton)e.Item.FindControl("lnkJobName");
                if (lnk != null && Request.QueryString["mode"] == "multi")
                {
                    lnk.Enabled = false;
                }
                RadCheckBox chksel = (RadCheckBox)e.Item.FindControl("chkSelect");
                if (chksel != null && Request.QueryString["mode"] != "multi")
                    chksel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        int iMessageCode = 0;
        string iMessageText = "";
        PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), ViewState["COMPONENTJOBID"].ToString(), null, 1, ref iMessageCode, ref iMessageText);
        string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
        Script += "</script>" + "\n";
        NameValueCollection nvc = new NameValueCollection();

        nvc.Add("lblJobCode", string.Empty);
        nvc.Add("lnkJobName", string.Empty);
        nvc.Add("lblComponentJobId", ViewState["COMPONENTJOBID"].ToString());
        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
