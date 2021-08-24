using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
  
public partial class RegistersEUMRVVesselTypeMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselTypeMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEUVesselType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselTypeMapping.aspx", "Search", "<i class=\"fas fa-search\"></i>", "Find");
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselTypeMapping.aspx", "Filter", "<i class=\"fas fa-eraser\"></i>", "Clear");
            
            MenuRegistersEUVesselType.AccessRights = this.ViewState;
            MenuRegistersEUVesselType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvEUVesselType.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersEUVesselType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvEUVesselType.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtCode.Text=string.Empty;
                txtVesselType.Text = string.Empty;
                txtEUVesselType.Text = string.Empty;
                BindData();
                gvEUVesselType.Rebind();             
            }
}

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool checkvalue(string EuVesselType, string VesselType,string TypeCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((TypeCode == null) || (TypeCode == ""))
            ucError.ErrorMessage = "Code is required.";

        if ((EuVesselType == null) || (EuVesselType == "") || (General.GetNullableGuid(EuVesselType)==null))
            ucError.ErrorMessage = "EU Vessel Type is required.";

        if ((VesselType == null) || (VesselType == "") || (General.GetNullableInteger(VesselType) == null))
            ucError.ErrorMessage = "Vessel Type is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    protected void gvEUVesselType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string EUVesselType, VesselType, code;

                VesselType = (((UserControlVesselType)e.Item.FindControl("ucVesselTypeAdd")).SelectedVesseltype.ToString().Trim());
                code = (((RadTextBox)e.Item.FindControl("txtEUVesselTypeCodeAdd")).Text.Trim());
                EUVesselType = (((RadComboBox)e.Item.FindControl("ddlEUVesselTypeAdd")).SelectedValue.ToString().Trim());

                if ((checkvalue(EUVesselType.ToString(), VesselType.Trim(), code.Trim())))
                {
                    PhoenixRegistersEUMRVVesselTypeMapping.InsertEUVesselTypeMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       General.GetNullableString(code),
                                                       General.GetNullableGuid(EUVesselType),
                                                       General.GetNullableInteger(VesselType)
                                                     );
                }
                BindData();
                gvEUVesselType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersEUMRVVesselTypeMapping.DeleteEUVesselTypeMapping((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVesselTypeMapid")).Text));
                BindData();
                gvEUVesselType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string EUVesselTypeid = ((RadComboBox)e.Item.FindControl("ddlEUVesselTypeEdit")).SelectedValue.ToString().Trim();
                string VesselTypeid = ((UserControlVesselType)e.Item.FindControl("ucVesselTypeEdit")).SelectedVesseltype.ToString().Trim();
                string VesselTypecode = ((RadTextBox)e.Item.FindControl("txtEUVesselTypeCodeEdit")).Text.Trim();
                string VesselTypeMapid = ((RadLabel)e.Item.FindControl("lblVesselTypeMapidEdit")).Text.Trim();

                if (checkvalue(EUVesselTypeid.ToString(), VesselTypeid.ToString(), VesselTypecode.ToString()))
                {

                    PhoenixRegistersEUMRVVesselTypeMapping.UpdateEUVesselTypeMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            General.GetNullableString(VesselTypecode),
                                                            General.GetNullableGuid(EUVesselTypeid),
                                                            General.GetNullableInteger(VesselTypeid),
                                                            General.GetNullableGuid(VesselTypeMapid)
                                                          );
                }

                BindData();
                gvEUVesselType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
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

    protected void gvEUVesselType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

          
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                
                RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlEUVesselTypeEdit");
                if (ddl != null)
                {
                    DataSet ds = PhoenixRegistersEUMRVVesselTypeMapping.ListEUVesselType(0);
                    ddl.DataSource = ds;
                    ddl.DataTextField = "FLDVESSELTYPE";
                    ddl.DataValueField = "FLDEUMRVVESSELTYPEID";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddl.SelectedIndex = 0;

                    ddl.SelectedValue = drv["FLDEUMRVVESSELTYPEID"].ToString();
                    //ddl1.SelectedVesseltype = vesselTypeid;
                }
                UserControlVesselType ucddl = (UserControlVesselType)e.Item.FindControl("ucVesselTypeEdit");
                if (ucddl != null)
                {
                    ucddl.SelectedVesseltype = drv["FLDVESSELTYPEID"].ToString();
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlEUVesselTypeAdd");
                if (ddl != null)
                {
                    DataSet ds = PhoenixRegistersEUMRVVesselTypeMapping.ListEUVesselType(0);
                    ddl.DataSource = ds;
                    ddl.DataTextField = "FLDVESSELTYPE";
                    ddl.DataValueField = "FLDEUMRVVESSELTYPEID";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddl.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvEUVesselType_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        gvEUVesselType.Rebind();
    }

  
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCODE", "FLDVESSELTYPE", "FLDTYPEDESCRIPTION" };
        string[] alCaptions = { "Code", "EU Vessel Type", "Vessel Type" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixRegistersEUMRVVesselTypeMapping.EUVesselTypeMappingSearch(General.GetNullableString(txtCode.Text.Trim()),
                                                General.GetNullableString(txtEUVesselType.Text.Trim()),
                                                General.GetNullableString(txtVesselType.Text.Trim()),
                                                 sortexpression,
                                                 sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                gvEUVesselType.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount
                                               );

        General.SetPrintOptions("gvEUVesselType", "Vessel Type Mapping", alCaptions, alColumns, ds);
        
        gvEUVesselType.DataSource = ds;
        gvEUVesselType.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDVESSELTYPE", "FLDTYPEDESCRIPTION" };
        string[] alCaptions = { "Code", "EU Vessel Type", "Vessel Type" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersEUMRVVesselTypeMapping.EUVesselTypeMappingSearch(General.GetNullableString(txtCode.Text.Trim()),
                                                General.GetNullableString(txtEUVesselType.Text.Trim()),
                                                General.GetNullableString(txtVesselType.Text.Trim()),
                                                 sortexpression,
                                                 sortdirection,
                                                1,
                                                iRowCount,
                                                 ref iRowCount,
                                                 ref iTotalPageCount
                                               );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"VesselTypeMapping.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Type Mapping</h3></td>");
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
    protected void gvEUVesselType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEUVesselType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
