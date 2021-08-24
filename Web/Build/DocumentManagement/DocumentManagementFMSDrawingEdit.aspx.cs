using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class DocumentManagementFMSDrawingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
                       
            if (Request.QueryString["FILENOID"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["FileNoID"] = Request.QueryString["FILENOID"].ToString();
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                FileNoEdit(Request.QueryString["FileNoID"].ToString());
                ddlvessel.SelectedVessel = ViewState["VESSELID"].ToString();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ddlvessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            } 

            //if ((Request.QueryString["vesselid"] != null)&& (Request.QueryString["vesselid"] != ""))
            //{
            //    ddlvessel.SelectedValue = int.Parse(Request.QueryString["vesselid"].ToString());
            //}
            MenuFMSFileNo.AccessRights = this.ViewState;
            MenuFMSFileNo.MenuList = toolbar.Show();
        }

      

    }

    protected void BindCategory()
    {
        string category = Request.QueryString["categoryid"];

        ddlCategory.DataSource = PhoenixRegisterFMSDrawing.FMSDrawingCategoryList();
        ddlCategory.DataTextField = "FLDDRAWINGCATEGORY";
        ddlCategory.DataValueField = "FLDFMSDRAWINGCATEGORY";
        ddlCategory.DataBind();
        ddlCategory.SelectedValue = category;
    }

    private void BindSubCategory(int? catagory)
    {
        ddlSubCategory.DataSource = PhoenixRegisterFMSDrawing.FMSDrawingSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubCategory.DataTextField = "FLDSUBCATEGORYNAME";
        ddlSubCategory.DataValueField = "FLDFMSDRAWINGSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategory.ClearSelection();
        BindSubCategory(General.GetNullableInteger(ddlCategory.SelectedValue));
    }

    protected void FMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidFileNo())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (ViewState["FileNoID"] != null)
                {
                    DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSDrawingInsert(txtDrawingno.Text
                                                        , new Guid(ViewState["FileNoID"].ToString())
                                                        , new Guid(ddlCategory.SelectedValue)
                                                        , null
                                                        , txtDrawingName.Text
                                                        , ddlvessel.SelectedValue);
                    
                }                
                ucStatus.Text = "Drawing No Updated";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private bool IsValidFileNo()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (ddlvessel.SelectedVessel.Equals(""))
            ucError.ErrorMessage = "Vessel Name is required.";

        if (txtDrawingno.Text.Equals(""))
            ucError.ErrorMessage = "Drawing No is required.";

        if (txtDrawingName.Text.Equals(""))
            ucError.ErrorMessage = "Drawing Name is required.";

        return (!ucError.IsError);
    }

    private void FileNoEdit(string FileNoID)
    {
        try
        {
            DataSet ds = PhoenixDocumentManagementFMSDrawings.EditFMSDrawingNo(new Guid(FileNoID));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtDrawingno.Text = dr["FLDSUBCATEGORYCODE"].ToString();
                txtDrawingName.Text = dr["FLDSUBCATEGORYNAME"].ToString();
                BindCategory();
                ddlCategory.SelectedValue = dr["FLDFMSDRAWINGCATEGORYID"].ToString();
                BindSubCategory(General.GetNullableInteger(ddlCategory.SelectedValue));               
                ddlSubCategory.SelectedValue = dr["FLDFMSDRAWINGSUBCATEGORYID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
