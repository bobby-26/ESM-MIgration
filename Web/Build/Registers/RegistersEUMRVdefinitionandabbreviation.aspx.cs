using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersEUMRVdefinitionandabbreviation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Lanchfrom"] != null)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
                if (Request.QueryString["Lanchfrom"].ToString() == "1")
                    toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL", ToolBarDirection.Right);
                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
            else
            {
                MenuProcedureDetailList.Visible = false;
            }

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVdefinitionandabbreviation.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSWS')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }
    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDABBREVIATION", "FLDEXPLANATION" };
                string[] alCaptions = { "Abbreviations", "Explanation" };

                DataTable dt = PhoenixRegistersEUMRVMesurementinstrument.Listdefinitionandabbreviation();
                General.ShowExcel("Definition and Abbreviations", dt, alColumns, alCaptions, null, string.Empty);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSWS_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {                
               string Abbreviation = (((RadTextBox)e.Item.FindControl("txtAbbreviationAdd")).Text.ToString().Trim());
                string Explanation = (((RadTextBox)e.Item.FindControl("txtExplanationAdd")).Text.ToString().Trim());

                if (!IsValidSeniority(Abbreviation, Explanation))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersEUMRVMesurementinstrument.EUMRVdefinitionandabbreviationInsert(Abbreviation, Explanation);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lbldtKey = ((RadLabel)e.Item.FindControl("lbldtKey")).Text;
                PhoenixRegistersEUMRVMesurementinstrument.EUMRVdefinitionandabbreviationDelete(new Guid(lbldtKey));
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSWS_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lbldtKeyEdit")).Text;
            string Abbreviation = (((RadTextBox)e.Item.FindControl("txtAbbreviationEdit")).Text.ToString().Trim());
            string Explanation = (((RadTextBox)e.Item.FindControl("txtExplanationEdit")).Text.ToString().Trim());
            
            if (!IsValidSeniority(Abbreviation, Explanation))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersEUMRVMesurementinstrument.EUMRVdefinitionandabbreviationUpdate(Abbreviation, Explanation, new Guid(dtkey));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSWS_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }
    private bool IsValidSeniority(String Abbreviation, String Explanation)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Abbreviation == "")
            ucError.ErrorMessage = "Abbreviation is required.";
        if (Explanation == "")
            ucError.ErrorMessage = "Explanation is required.";
        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSWS_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDABBREVIATION", "FLDEXPLANATION" };
        string[] alCaptions = { "Abbreviations", "Explanation" };

        DataTable dt = PhoenixRegistersEUMRVMesurementinstrument.Listdefinitionandabbreviation();
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSWS", "Definition and Abbreviations", alCaptions, alColumns, ds);
        gvSWS.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (Request.QueryString["view"] != null)
            {
                gvSWS.Columns[3].Visible = false;
            }
        }
    }
    protected void Rebind()
    {
        gvSWS.SelectedIndexes.Clear();
        gvSWS.EditIndexes.Clear();
        gvSWS.DataSource = null;
        gvSWS.Rebind();
    }
}
