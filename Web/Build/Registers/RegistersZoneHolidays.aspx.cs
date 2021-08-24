using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegistersZoneHolidays : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
         
            toolbar.AddImageButton("../Registers/RegistersZoneHolidays.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersZoneHolidays.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuRegistersholiday.AccessRights = this.ViewState;
            MenuRegistersholiday.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvzoneholiday.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvzoneholiday.SelectedIndexes.Clear();
        gvzoneholiday.EditIndexes.Clear();
        gvzoneholiday.DataSource = null;
        gvzoneholiday.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

       

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersZoneHolidays.Searchzoneholiday(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(ucZone.SelectedZone),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvzoneholiday.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

     

        gvzoneholiday.DataSource = ds;
        gvzoneholiday.VirtualItemCount = iRowCount;



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvzoneholiday_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvzoneholiday.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ucZone_Changed(object sender, EventArgs e)
    {

        gvzoneholiday.Rebind();
    }
    protected void Registersholiday_TabStripCommand(object sender, EventArgs e)
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
          
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucZone.SelectedZone = "";
               
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
    private void Insertzoneholidays(int zoneid,string date, string description)
    {
        PhoenixRegistersZoneHolidays.Insertzoneholidays(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, zoneid, DateTime.Parse(date), description);
        ucStatus.Text = "Information Added";
    }
    private void Updatezoneholidays(int id, int zoneid, string date, string description)
    {
        PhoenixRegistersZoneHolidays.Updatezoneholidays(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                id, zoneid, DateTime.Parse(date), description);
        ucStatus.Text = "Information Updated";

    }

    protected void gvzoneholiday_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {



            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                Insertzoneholidays(
                    int.Parse(((UserControlZone)e.Item.FindControl("ddlzoneadd")).SelectedZone),
                     ((UserControlDate)e.Item.FindControl("ucdateholidayadd")).Text,
                     ((RadTextBox)e.Item.FindControl("txtdescriptionadd")).Text
                    );
                Rebind();

            }
           else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                Updatezoneholidays((Int32.Parse(((RadLabel)e.Item.FindControl("lblIdedit")).Text)),
                     int.Parse(((UserControlZone)e.Item.FindControl("ddlzoneEdit")).SelectedZone),
                      ((UserControlDate)e.Item.FindControl("ucdateholidayedit")).Text,
                      ((RadTextBox)e.Item.FindControl("txtdescriptionEdit")).Text
                     );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
    private void deletezoneholidays(int id)
    {
        PhoenixRegistersZoneHolidays.deletezoneholidays(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    }

    protected void gvzoneholiday_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);


        }
        if (e.Item.IsInEditMode)
        {
            UserControlZone uczone = (UserControlZone)e.Item.FindControl("ddlzoneEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (uczone != null) uczone.SelectedZone = DataBinder.Eval(e.Item.DataItem, "FLDZONEID").ToString();
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            deletezoneholidays(Int32.Parse(ViewState["ID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}