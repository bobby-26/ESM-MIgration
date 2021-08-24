using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersInstitutionMapping : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersInstitutionMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInstitutionMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersInstitutionMapping.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersInstitutionMapping.AccessRights = this.ViewState;
            MenuRegistersInstitutionMapping.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvInstitutionMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDINSTITUTIONMAPPINGID", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersInstitutionMapping.InstitutionMappingSearch(General.GetNullableString(txtCode.Text)
                                                                        , General.GetNullableString(txtName.Text)
                                                                        , sortexpression
                                                                        , sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvInstitutionMapping.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=InstitutionMapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Institution Mapping</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }
    protected void MenuRegistersInstitutionMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvInstitutionMapping.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDINSTITUTIONMAPPINGID", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersInstitutionMapping.InstitutionMappingSearch(General.GetNullableString(txtCode.Text)
                                                                          , General.GetNullableString(txtName.Text)
                                                                          , sortexpression
                                                                          , sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvInstitutionMapping.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvInstitutionMapping", "Institution Mapping", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvInstitutionMapping.DataSource = ds;
            gvInstitutionMapping.VirtualItemCount = iRowCount;
        }
        else
        {
            gvInstitutionMapping.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {        
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvInstitutionMapping.Rebind();
    }

  
    private void InsertInstitutionMapping(string institutionid, string code)
    {

        PhoenixRegistersInstitutionMapping.InsertInstitutionMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Int32.Parse(institutionid.ToString()), code);
    }
    private void UpdateInstitutionMapping(string shortcodeid, string institutionid, string shortcode)
    {
        PhoenixRegistersInstitutionMapping.UpdateInstitutionMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Int32.Parse(shortcodeid.ToString()), Int32.Parse(institutionid.ToString()), shortcode);
        ucStatus.Text = "Information updated";
    }
    private bool IsValidInstitution(string institutionid, string shortcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (institutionid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "institution is required.";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        return (!ucError.IsError);
    }
    
    protected void BindVenueDetails(object sender, EventArgs e)
    {
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvInstitutionMapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
           
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                UserControlAddressType institution = ((UserControlAddressType)e.Item.FindControl("ucInstitutionAdd"));
                if (!IsValidInstitution(institution.SelectedAddress, ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertInstitutionMapping(institution.SelectedAddress,
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text

                );
                BindData();
                gvInstitutionMapping.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersInstitutionMapping.DeleteInstitutionMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(((RadLabel)e.Item.FindControl("lblInstituteId")).Text));
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string shortcodeid = ((RadLabel)e.Item.FindControl("lblInstitutionIdEdit")).Text;
                string intsituteid = ((RadLabel)e.Item.FindControl("lblInstitutionLocIdEdit")).Text;
                if (!IsValidInstitution(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                                            intsituteid))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateInstitutionMapping(shortcodeid, intsituteid,
                    ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text);
               
                BindData();
                gvInstitutionMapping.Rebind();
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

    protected void gvInstitutionMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInstitutionMapping.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvInstitutionMapping_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlAddressType institutionid = ((UserControlAddressType)e.Item.FindControl("ucinstitutionEdit"));
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (institutionid != null) institutionid.SelectedAddress = drv["FLDinstitutionID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
