using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersCargo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCargo.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCargo')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCargo.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCargo.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersCargo.AccessRights = this.ViewState;
            MenuRegistersCargo.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = 1;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CargoTypeCode"] = ucCargo.SelectedCargoType;
                gvCargo.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCargo.SelectedIndexes.Clear();
        gvCargo.EditIndexes.Clear();
        gvCargo.DataSource = null;
        gvCargo.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDCARGONAME", "FLDCARGOTYPENAME", "FLDTYPEDESCRIPTION" };
        string[] alCaptions = { "Short Name", "Name", "Cargo Type", "Vessel Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCargo.CargoSearch(
            General.GetNullableGuid(ucCargo.SelectedCargoType.ToString()),
            txtCargoName.Text, null,
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCargo.PageSize,
            ref iRowCount,
            ref iTotalPageCount, General.GetNullableInteger(ucVesselTypeSearch.SelectedHard));

        General.SetPrintOptions("gvCargo", "Cargo", alCaptions, alColumns, ds);

        gvCargo.DataSource = ds;
        gvCargo.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void RegistersCargo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucCargo.SelectedCargoType = "";
                txtCargoName.Text = "";
                ucVesselTypeSearch.SelectedHard = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCargo(string name, string shortname, string type, string vesseltype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Short Name is required.";
       
        if ((name == null) || (name == ""))
          ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableGuid(type) == null)
            ucError.ErrorMessage = "Cargo Type is required.";

        if (vesseltype == "")
            ucError.ErrorMessage = "Vessel Type is required.";

        return (!ucError.IsError);
    }

    protected void gvCargo_ItemCommand(object sender, GridCommandEventArgs e)
    {   
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string CargoType, CargoName, CargoShortName;
                CargoType = ((UserControlCargoType)e.Item.FindControl("ucCargoType1")).SelectedCargoType;
                CargoName = (((RadTextBox)e.Item.FindControl("txtCargoNameAdd")).Text);
                CargoShortName = (((RadTextBox)e.Item.FindControl("txtCargoShortNameAdd")).Text);

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListAdd");
                string vesseltypelist = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        vesseltypelist += li.Value + ",";
                    }
                }

                if (!IsValidCargo(CargoName, CargoShortName, CargoType, vesseltypelist))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersCargo.InsertCargo(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((UserControlCargoType)e.Item.FindControl("ucCargoType1")).SelectedCargoType),
                    (((RadTextBox)e.Item.FindControl("txtCargoNameAdd")).Text),
                    (((RadTextBox)e.Item.FindControl("txtCargoShortNameAdd")).Text),
                    null,
                    ',' + vesseltypelist);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersCargo.DeleteCargo(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,  
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoCode")).Text));
                Rebind();
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

    protected void gvCargo_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                UserControlCargoType ucCargoType = (UserControlCargoType)e.Item.FindControl("ucCargoTypeEdit");
                if (ucCargoType != null) ucCargoType.SelectedCargoType = drv["FLDCARGOTYPECODE"].ToString();

                //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                //{
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                //}

                RadCheckBoxList ucVesselTypeList = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListEdit");
                if (ucVesselTypeList != null)
                {
                    ucVesselTypeList.DataSource = PhoenixRegistersHard.ListHard(1, 81);
                    ucVesselTypeList.DataBindings.DataTextField = "FLDHARDNAME";
                    ucVesselTypeList.DataBindings.DataValueField = "FLDHARDCODE";
                    ucVesselTypeList.DataBind();

                    RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListEdit");
                    foreach (ButtonListItem li in chk.Items)
                    {
                        string[] slist = drv["FLDVESSELTYPELIST"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
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

                RadCheckBoxList ucVesselTypeList = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListAdd");
                if (ucVesselTypeList != null)
                {
                    ucVesselTypeList.DataSource = PhoenixRegistersHard.ListHard(1, 81);
                    ucVesselTypeList.DataBindings.DataTextField = "FLDHARDNAME";
                    ucVesselTypeList.DataBindings.DataValueField = "FLDHARDCODE";
                    ucVesselTypeList.DataBind();
                } 
            }
        }
        catch (Exception ex)
        {            
            throw ex;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSHORTNAME", "FLDCARGONAME", "FLDCARGOTYPENAME", "FLDTYPEDESCRIPTION" };
        string[] alCaptions = { "Short Name", "Name", "Cargo Type", "Vessel Type" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCargo.CargoSearch(
            General.GetNullableGuid(ucCargo.SelectedCargoType.ToString()), txtCargoName.Text,
            null, sortexpression,
            sortdirection, 1,
            iRowCount, ref iRowCount, ref iTotalPageCount, 
            General.GetNullableInteger(ucVesselTypeSearch.SelectedHard));
      
        Response.AddHeader("Content-Disposition", "attachment; filename=\"Cargo.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cargo</h3></td>");
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
    protected void gvCargo_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string CargoType, CargoName, CargoShortName;
            CargoType = (((UserControlCargoType)e.Item.FindControl("ucCargoTypeEdit")).SelectedCargoType);
            CargoName = (((RadTextBox)e.Item.FindControl("txtCargoNameEdit")).Text);
            CargoShortName = (((RadTextBox)e.Item.FindControl("txtCargoShortNameEdit")).Text);

            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListEdit");
            string vesseltypelist = "";
            foreach (ButtonListItem li in chk.Items)
            {
                if (li.Selected)
                {
                    vesseltypelist += li.Value + ",";
                }
            }

            if (!IsValidCargo(CargoName, CargoShortName, CargoType, vesseltypelist))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersCargo.UpdateCargo(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(((UserControlCargoType)e.Item.FindControl("ucCargoTypeEdit")).SelectedCargoType),
                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoCodeEdit")).Text),
                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCargoNameEdit")).Text),
                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCargoShortNameEdit")).Text),
                null,
                ',' + vesseltypelist);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCargo.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargo_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
