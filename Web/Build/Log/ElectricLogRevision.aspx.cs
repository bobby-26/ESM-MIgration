using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogRevision : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
      //  ShowToolBar();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "Log/ElectricLogRevisionAdd.aspx'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //toolbarmain.AddFontAwesomeButton("../Log/ElectricLogRevision.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        //toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvRevision')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        gvTabStrip.MenuList = toolbarmain.Show();
        gvTabStrip.AccessRights = this.ViewState;

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRevision.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     #region Grid Events
    protected void gvRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
		DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
		gvRevision.DataSource = PhoenixElog.PhoenixLogRevisionSearch(usercode, vesselId, 1, 100, ref iRowCount, ref iTotalPageCount);
        gvRevision.VirtualItemCount = iRowCount;
    }

    protected void gvRevision_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {
           
        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;
            HyperLink hlnkAddedDate = (HyperLink)e.Item.FindControl("hlnkAddedDate");

            if (hlnkAddedDate != null)
            {
                string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Log/ElectricLogRevisionView.aspx?date={1}&revisionNo={2}', 'true'); return false", Session["sitepath"], drv["FLDREVISIONDATE"].ToString(), drv["FLDREVISIONNO"].ToString());
                hlnkAddedDate.Attributes.Add("onclick", script);
                //"&FormName=" + dr["FLDFORMDESIGNNAME"].ToString() + "
            }
        }
        if (e.Item is GridDataItem)
        {
            LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");
            
        }
		if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                //if (string.IsNullOrWhiteSpace(drv["FLDREVISIONID"].ToString()) == false)
                //{
                //    //string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Log/ElectricLogRevisionAdd.aspx?id={1}', 'true'); return false", Session["sitepath"], drv["FLDREVISIONID"].ToString());
                //    //editBtn.Attributes.Add("onclick", script);
                //}
            }
        }
    }

    protected void gvRevision_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.CommandName.ToUpper().Equals("LINK"))
            {
                //string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Log/ElectricLogRevisionView.aspx?id={1}', 'true'); return false", Session["sitepath"], drv["FLDREVISIONID"].ToString());
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdSave");
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                Guid Id = new Guid(e.CommandArgument.ToString());
                gvRevision.Rebind();
            }
            if (e.CommandName == "VIEW")
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    #endregion


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
	try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}