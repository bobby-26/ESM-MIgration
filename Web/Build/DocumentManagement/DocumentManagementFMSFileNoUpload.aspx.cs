using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DocumentManagementFMSFileNoUpload : PhoenixBasePage
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
                ViewState["FileNoID"] = Request.QueryString["FileNoID"].ToString();
                FileNoEdit(Request.QueryString["FileNoID"].ToString());
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuFMSFileNo.AccessRights = this.ViewState;
            MenuFMSFileNo.MenuList = toolbar.Show();
            BindFileNo();
            ViewState["DTKEY"] = null;

        }
        BindMenu();

    }

    protected void BindFileNo()
    {
        ddlFileNo.DataSource = PhoenixRegisterFMSMail.FMSFilenoList(1);
        ddlFileNo.DataTextField = "FLDFILENODETAILS";
        ddlFileNo.DataValueField = "FLDFMSMAILFILENOID";
        ddlFileNo.DataBind();
        ddlFileNo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void FMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? guid = Guid.Empty;

                if (!IsValidFileNo())
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.Files["txtFileUpload"].ContentLength > 0)
                {

                    PhoenixRegisterFMSMail.FMSInsertFileNoupload(ddlFileNo.SelectedValue
                                                           , int.Parse(ddlvessel.SelectedVessel)
                                                           , txtDescription.Text
                                                           , ref guid);
                    ViewState["DTKEY"] = guid;

                    if (ViewState["DTKEY"].ToString() != null)
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["DTKEY"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, "", string.Empty, "FMSFILENO", int.Parse(ddlvessel.SelectedVessel));

                        lblMessage.Text = "Form is uploaded.";
                        lblMessage.ForeColor = System.Drawing.Color.Blue;
                        lblMessage.Visible = true;

                    }
                }
               
                //BindMenu();

            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                // Response.Redirect("../Registers/RegistersFMSFileNoList.aspx");
                Response.Redirect("../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT + "&type=FMSFILENO" + "&BYVESSEL=true&cmdname=FMSFILENOUPLOAD&VESSELID=" + ddlvessel.SelectedVessel);
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

        //String script = String.Format("javascript:fnReloadList('code1');");
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void cmdUpload_Click(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }
    }


    public void BindMenu()
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        if (ViewState["DTKEY"] != null)
        {
            toolbar.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);

        }
        
        MenuFMSFileNo.AccessRights = this.ViewState;
        MenuFMSFileNo.MenuList = toolbar.Show();
    }

    private bool IsValidFileNo()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlvessel.SelectedValue.Equals(""))
            ucError.ErrorMessage = "Vessel Name is required.";

        if (ddlFileNo.SelectedValue.Equals(""))
            ucError.ErrorMessage = "File No is required.";

        if(Request.Files["txtFileUpload"].ContentLength == 0)
            ucError.ErrorMessage = "File upload is required.";

        return (!ucError.IsError);
    }
    private void FileNoEdit(string FileNoID)
    {
        try
        {
            DataSet ds = PhoenixRegisterFMSMail.EditFMSFileNo(new Guid(FileNoID));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtDescription.Text = dr["FLDFILENODESCRIPTION"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
