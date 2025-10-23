using System;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using ToDoList.GUI.Forms;

namespace ToDoList.GUI.Tests
{
    /// <summary>
    /// Test form ?? m? Cuculist Focus Mode
    /// </summary>
    public static class OpenCuculistFocusTest
    {
        public static void OpenFocusMode()
        {
            try
            {
                using (var context = new ToDoListContext())
                {
                    // L?y project ??u ti�n
                    var project = context.Projects
                        .Where(p => p.IsArchived != true)
                        .FirstOrDefault();

                    if (project == null)
                    {
                        MessageBox.Show("Kh�ng t�m th?y project n�o! Vui l�ng t?o project tr??c.", 
                            "Th�ng b�o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // L?y task ??u ti�n ch?a ho�n th�nh
                    var firstTask = context.Tasks
                        .FirstOrDefault(t => t.ProjectId == project.ProjectId && 
                                           t.Status != "Completed" && 
                                           t.IsDeleted != true);

                    // M? Cuculist Focus Mode (Mini Form)
                    using (var miniForm = new CuculistMiniForm(project, firstTask, TimeSpan.Zero))
                    {
                        MessageBox.Show($"? ?ang m? Cuculist Focus Mode...\n\n" +
                            $"?? Project: {project.ProjectName}\n" +
                            $"?? Task: {firstTask?.Title ?? "Kh�ng c� task"}", 
                            "Cuculist Focus Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        miniForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"? L?i khi m? Cuculist Focus Mode:\n\n{ex.Message}\n\n" +
                    $"Stack Trace:\n{ex.StackTrace}", 
                    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
