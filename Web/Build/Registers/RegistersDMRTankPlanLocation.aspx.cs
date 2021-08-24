using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDMRTankPlanLocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRTankPlanLocation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvLocation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        UcVessel.Enabled = false;
                    }
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                    UcVessel.Enabled = false;
                }
                //BindData();
                gvLocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvLocation.Rebind();
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

    protected void gvLocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text),
                    (((RadTextBox)e.Item.FindControl("txtTaskNameAdd")).Text),
                    ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text,
                    ((RadComboBox)e.Item.FindControl("ddlHorizontalValueAdd")).Text,
                    ((RadComboBox)e.Item.FindControl("ddlVerticalValueAdd")).Text,
                    ((RadRadioButtonList)e.Item.FindControl("rblProductTypeAdd")).SelectedValue))
                    return;
                CheckBoxList chka = (CheckBoxList)e.Item.FindControl("chkVesselListAdd");
                string VList = "";
                string VesselList = "";
                foreach (ListItem li in chka.Items)
                {
                    if (li.Selected)
                    {
                        VList += li.Value + ",";
                    }
                }

                if (VList != "")
                {
                    VesselList = "," + VList;
                }
                if (VesselList == "")
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    if ((VesselList == null) || (VesselList == ""))
                        ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;

                }
                foreach (ListItem li in chka.Items)
                {
                    if (li.Selected)
                    {
                        PhoenixRegistersDMRTankPlanLocation.DMRTankPlanLocationInsert(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            ((RadTextBox)e.Item.FindControl("txtTaskNameAdd")).Text,
                            ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text,
                            int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text),
                            null,
                            int.Parse(((RadComboBox)e.Item.FindControl("ddlHorizontalValueAdd")).SelectedValue),
                            int.Parse(((RadComboBox)e.Item.FindControl("ddlVerticalValueAdd")).SelectedValue),
                            ((RadCheckBox)e.Item.FindControl("chkMethanolTankYNAdd")).Checked == true ? 1 : 0,
                            General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlTankLocAdd")).SelectedValue),
                            General.GetNullableInteger(li.Value),
                            General.GetNullableString(((RadRadioButtonList)e.Item.FindControl("rblProductTypeAdd")).SelectedValue));
                    }
                }
                BindData();
                gvLocation.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMRTankPlanLocation.DMRTankPlanLocationDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblLocationId")).Text));
                BindData();
                gvLocation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text),
                (((RadTextBox)e.Item.FindControl("txtTaskNameEdit")).Text),
                ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text,
                ((RadComboBox)e.Item.FindControl("ddlHorizontalValueEdit")).SelectedValue,
                ((RadComboBox)e.Item.FindControl("ddlVerticalValueEdit")).SelectedValue,
                ((RadRadioButtonList)e.Item.FindControl("rblProductTypeEdit")).SelectedValue))
                    return;


                PhoenixRegistersDMRTankPlanLocation.DMRTankPlanLocationUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblLocationIdEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtTaskNameEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text),
                    null,
                    int.Parse(((RadComboBox)e.Item.FindControl("ddlHorizontalValueEdit")).SelectedValue),
                    int.Parse(((RadComboBox)e.Item.FindControl("ddlVerticalValueEdit")).SelectedValue),
                    ((RadCheckBox)e.Item.FindControl("chkMethanolTankYNEdit")).Checked == true ? 1 : 0,
                    General.GetNullableString(((RadComboBox)e.Item.FindControl("ddlTankLocEdit")).SelectedValue),
                    General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselEditID")).Text),
                    General.GetNullableString(((RadRadioButtonList)e.Item.FindControl("rblProductTypeEdit")).SelectedValue));
                BindData();
                gvLocation.Rebind();
            }
        
        }
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool checkvalue(string shortName, string TankName, string sortOrder, string horizontalorder, string verticalorder, string productType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortName == null) || (shortName == ""))
            ucError.ErrorMessage = "Short name is required.";

        if ((TankName == null) || (TankName == ""))
            ucError.ErrorMessage = "Tank name is required.";

        if ((sortOrder == null) || (sortOrder == ""))
            ucError.ErrorMessage = "Sort order is required.";
        if ((horizontalorder == null) || (horizontalorder == ""))
            ucError.ErrorMessage = "Horizontal Sort order is required.";
        if ((verticalorder == null) || (verticalorder == ""))
            ucError.ErrorMessage = "Vertical Sort order is required.";
        if (productType == "")
            ucError.ErrorMessage = "Product type is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHORIZONTALNAME", "FLDVERTICALVALUE", "FLDSHORTNAME", "FLDLOCATIONNAME", "FLDVESSELNAMELIST", "FLDSORTORDER" };
        string[] alCaptions = { "Horizontal Value", "Vertical Value", "Short Code", "Description", "Vessel", "Sort Order" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();

        DataSet ds = PhoenixRegistersDMRTankPlanLocation.DMRTankPlanLocationSearch(General.GetNullableInteger(vesselid), "",
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvLocation.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvLocation", "Tank Plan Location", alCaptions, alColumns, ds);
        gvLocation.DataSource = ds;
        gvLocation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void gvLocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
             RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");
            LinkButton ImgVesselList = (LinkButton)e.Item.FindControl("ImgVesselList");
            
            if (ImgVesselList != null)
            {
                if (lblVessel != null)
                {
                    if (lblVessel.Text != "")
                    {
                        ImgVesselList.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucVesselList");
                        if (uct != null)
                        {
                            ImgVesselList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            ImgVesselList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                            //ImgVesselList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            //ImgVesselList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgVesselList.Visible = false;
                }
            }


            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                CheckBoxList chkVesselListEdit = (CheckBoxList)e.Item.FindControl("chkVesselListEdit");
                if (chkVesselListEdit != null)
                {
                    chkVesselListEdit.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    chkVesselListEdit.DataTextField = "FLDVESSELNAME";
                    chkVesselListEdit.DataValueField = "FLDVESSELID";
                    chkVesselListEdit.DataBind();

                    CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkVesselListEdit");
                    foreach (ListItem li in chk.Items)
                    {
                        string[] slist = drv["FLDVESSELLIST"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        foreach (ListItem li in chk.Items)
                        {
                            if (li.Value != PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                            {
                                li.Enabled = false;
                            }
                            else
                            {
                                li.Enabled = true;
                            }
                        }
                    }
                }

                RadComboBox ddlHorizontalValueEdit = (RadComboBox)e.Item.FindControl("ddlHorizontalValueEdit");
                if (ddlHorizontalValueEdit != null)
                    ddlHorizontalValueEdit.SelectedValue = drv["FLDHORIZONTALVALUE"].ToString();

                RadComboBox ddlVerticalValueEdit = (RadComboBox)e.Item.FindControl("ddlVerticalValueEdit");
                if (ddlVerticalValueEdit != null)
                    ddlVerticalValueEdit.SelectedValue = drv["FLDVERTICALVALUE"].ToString();

                RadComboBox ddlTankLocEdit = (RadComboBox)e.Item.FindControl("ddlTankLocEdit");
                if (ddlTankLocEdit != null)
                    ddlTankLocEdit.SelectedValue = drv["FLDPORTORSTBD"].ToString();

                RadRadioButtonList rblProductTypeEdit = (RadRadioButtonList)e.Item.FindControl("rblProductTypeEdit");
                if (rblProductTypeEdit != null)
                    rblProductTypeEdit.SelectedValue = drv["FLDPRODUCTTYPESHORTNAME"].ToString();

            }
            else if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
                CheckBoxList chkVesselListAdd = (CheckBoxList)e.Item.FindControl("chkVesselListAdd");
                if (chkVesselListAdd != null)
                {
                    chkVesselListAdd.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    chkVesselListAdd.DataTextField = "FLDVESSELNAME";
                    chkVesselListAdd.DataValueField = "FLDVESSELID";
                    chkVesselListAdd.DataBind();
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    foreach (ListItem li in chkVesselListAdd.Items)
                    {
                        if (li.Value != PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                        {
                            li.Enabled = false;
                        }
                        else
                        {
                            li.Enabled = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLocation_Sorting(object sender, DataGridSortCommandEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDHORIZONTALNAME", "FLDVERTICALVALUE", "FLDSHORTNAME", "FLDLOCATIONNAME", "FLDVESSELNAMELIST", "FLDSORTORDER" };
        string[] alCaptions = { "Horizontal Value", "Vertical Value", "Short Code", "Description", "Vessel", "Sort Order" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string vesselid;

        if (ViewState["VESSELID"].ToString().Equals(""))
            vesselid = UcVessel.SelectedVessel;
        else
            vesselid = ViewState["VESSELID"].ToString();
        ds = PhoenixRegistersDMRTankPlanLocation.DMRTankPlanLocationSearch(General.GetNullableInteger(vesselid), "",
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"DMR Tank Plan Location.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Tank Plan Location</h3></td>");
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

    //protected void gvLocation_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //        RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");
    //        LinkButton ImgVesselList = (LinkButton)e.Item.FindControl("ImgVesselList");
            
    //        if (ImgVesselList != null)
    //        {
    //            if (lblVessel != null)
    //            {
    //                if (lblVessel.Text != "")
    //                {
    //                    ImgVesselList.Visible = true;
    //                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucVesselList");
    //                    if (uct != null)
    //                    {
    //                        ImgVesselList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //                        ImgVesselList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    //                    }
    //                }
    //                else
    //                    ImgVesselList.Visible = false;
    //            }
    //        }
       
    //    if (e.Item is GridDataItem)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        CheckBoxList chkVesselListEdit = (CheckBoxList)e.Item.FindControl("chkVesselListEdit");
    //        if (chkVesselListEdit != null)
    //        {
    //            chkVesselListEdit.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //            chkVesselListEdit.DataTextField = "FLDVESSELNAME";
    //            chkVesselListEdit.DataValueField = "FLDVESSELID";
    //            chkVesselListEdit.DataBind();

    //            CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkVesselListEdit");
    //            foreach (ListItem li in chk.Items)
    //            {
    //                string[] slist = drv["FLDVESSELLIST"].ToString().Split(',');
    //                foreach (string s in slist)
    //                {
    //                    if (li.Value.Equals(s))
    //                    {
    //                        li.Selected = true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    else if (e.Item is GridFooterItem)
    //    {
    //        CheckBoxList chkVesselListAdd = (CheckBoxList)e.Item.FindControl("chkVesselListAdd");
    //        if (chkVesselListAdd != null)
    //        {
    //            chkVesselListAdd.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //            chkVesselListAdd.DataTextField = "FLDVESSELNAME";
    //            chkVesselListAdd.DataValueField = "FLDVESSELID";
    //            chkVesselListAdd.DataBind();
    //        }
    //    }
    //}


    protected void gvLocation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLocation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
