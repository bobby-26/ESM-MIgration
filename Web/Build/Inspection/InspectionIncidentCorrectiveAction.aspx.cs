using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class InspectionIncidentCorrectiveAction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        
        
        if (!IsPostBack)
        {
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");                
            if (Filter.CurrentSelectedIncidentMenu == null)
            {
                MenuCARGeneral.MenuList = toolbar.Show();
            }            
            BindImmediateAction();
                
        }              
    }
    private void BindImmediateAction()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
            txtImmediateaction.Text = ds.Tables[0].Rows[0]["FLDIMMEDIATEACTION"].ToString();
    }
    
    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (Filter.CurrentIncidentID != null && Filter.CurrentIncidentID.ToString() != string.Empty)
            {
                PhoenixInspectionIncident.UpdateIncidentImmediateAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Filter.CurrentIncidentID),
                    General.GetNullableString(txtImmediateaction.Text));

                ucStatus.Text = "Immediate Action updated.";
            }
            else
            {
                ucError.ErrorMessage = "Please record Incident details before recording Immediate action.";
                ucError.Visible = true;
            }
        }
    }    
        
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }    
}
