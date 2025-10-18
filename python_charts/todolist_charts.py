import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
import numpy as np
import pyodbc
import json
import sys
from datetime import datetime, timedelta
import matplotlib.patches as mpatches
from matplotlib.patches import Rectangle
import matplotlib.dates as mdates

class ToDoListChartGenerator:
    def __init__(self, connection_string):
        """Initialize with database connection"""
        self.connection_string = connection_string
        plt.style.use('dark_background')
        sns.set_palette("husl")
        
    def connect_database(self):
        """Connect to SQL Server database"""
        try:
            self.connection = pyodbc.connect(self.connection_string)
            return True
        except Exception as e:
            print(f"Database connection error: {e}")
            return False
    
    def get_data(self, query):
        """Execute query and return DataFrame"""
        try:
            return pd.read_sql(query, self.connection)
        except Exception as e:
            print(f"Query error: {e}")
            return pd.DataFrame()
    
    def create_project_progress_chart(self, save_path="project_progress.png"):
        """?? Create beautiful project progress chart"""
        query = """
        SELECT 
            p.ProjectName,
            p.ColorCode,
            COUNT(t.TaskID) as TotalTasks,
            COUNT(CASE WHEN t.Status = 'Completed' THEN 1 END) as CompletedTasks,
            COUNT(CASE WHEN t.Status = 'In Progress' THEN 1 END) as InProgressTasks,
            COUNT(CASE WHEN t.Status = 'Pending' THEN 1 END) as PendingTasks
        FROM Projects p
        LEFT JOIN Tasks t ON p.ProjectID = t.ProjectID AND t.IsDeleted = 0
        WHERE p.IsArchived = 0
        GROUP BY p.ProjectID, p.ProjectName, p.ColorCode
        HAVING COUNT(t.TaskID) > 0
        ORDER BY COUNT(t.TaskID) DESC
        """
        
        df = self.get_data(query)
        if df.empty:
            print("No project data found")
            return
        
        # Calculate completion rate
        df['CompletionRate'] = (df['CompletedTasks'] / df['TotalTasks'] * 100).round(1)
        
        # Create figure with dark theme
        fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(16, 8))
        fig.patch.set_facecolor('#1a1a1a')
        
        # Chart 1: Horizontal bar chart
        colors = ['#FF6B6B', '#4ECDC4', '#45B7D1', '#96CEB4', '#FFEAA7', '#DDA0DD']
        bars = ax1.barh(df['ProjectName'], df['CompletionRate'], 
                       color=colors[:len(df)], alpha=0.8)
        
        ax1.set_xlabel('Completion Rate (%)', fontsize=12, color='white')
        ax1.set_title('?? Project Completion Rates', fontsize=14, fontweight='bold', color='white')
        ax1.set_facecolor('#2d2d2d')
        ax1.tick_params(colors='white')
        
        # Add percentage labels on bars
        for i, (bar, rate) in enumerate(zip(bars, df['CompletionRate'])):
            ax1.text(bar.get_width() + 1, bar.get_y() + bar.get_height()/2, 
                    f'{rate}%', va='center', color='white', fontweight='bold')
        
        # Chart 2: Stacked bar chart
        width = 0.6
        x = np.arange(len(df))
        
        p1 = ax2.bar(x, df['CompletedTasks'], width, label='? Completed', color='#4CAF50')
        p2 = ax2.bar(x, df['InProgressTasks'], width, bottom=df['CompletedTasks'], 
                    label='?? In Progress', color='#FF9800')
        p3 = ax2.bar(x, df['PendingTasks'], width, 
                    bottom=df['CompletedTasks'] + df['InProgressTasks'],
                    label='? Pending', color='#F44336')
        
        ax2.set_xlabel('Projects', fontsize=12, color='white')
        ax2.set_ylabel('Number of Tasks', fontsize=12, color='white')
        ax2.set_title('?? Task Status Distribution by Project', fontsize=14, fontweight='bold', color='white')
        ax2.set_xticks(x)
        ax2.set_xticklabels(df['ProjectName'], rotation=45, ha='right', color='white')
        ax2.legend(loc='upper right')
        ax2.set_facecolor('#2d2d2d')
        ax2.tick_params(colors='white')
        
        # Add value labels on stacked bars
        for i in range(len(df)):
            completed = df.iloc[i]['CompletedTasks']
            in_progress = df.iloc[i]['InProgressTasks']
            pending = df.iloc[i]['PendingTasks']
            
            if completed > 0:
                ax2.text(i, completed/2, str(completed), ha='center', va='center', 
                        color='white', fontweight='bold')
            if in_progress > 0:
                ax2.text(i, completed + in_progress/2, str(in_progress), ha='center', va='center', 
                        color='white', fontweight='bold')
            if pending > 0:
                ax2.text(i, completed + in_progress + pending/2, str(pending), ha='center', va='center', 
                        color='white', fontweight='bold')
        
        plt.tight_layout()
        plt.savefig(save_path, dpi=300, bbox_inches='tight', facecolor='#1a1a1a')
        plt.close()
        print(f"Project progress chart saved to {save_path}")
    
    def create_time_analysis_chart(self, save_path="time_analysis.png"):
        """? Create time analysis charts"""
        query = """
        SELECT 
            Priority,
            Status,
            COUNT(*) as TaskCount,
            AVG(CAST(EstimatedMinutes AS FLOAT)) as AvgMinutes,
            SUM(CAST(EstimatedMinutes AS FLOAT)) as TotalMinutes
        FROM Tasks 
        WHERE IsDeleted = 0 AND EstimatedMinutes IS NOT NULL
        GROUP BY Priority, Status
        ORDER BY Priority, Status
        """
        
        df = self.get_data(query)
        if df.empty:
            print("No time data found")
            return
        
        fig, ((ax1, ax2), (ax3, ax4)) = plt.subplots(2, 2, figsize=(16, 12))
        fig.patch.set_facecolor('#1a1a1a')
        
        # Chart 1: Priority distribution pie chart
        priority_data = df.groupby('Priority')['TaskCount'].sum()
        colors_pie = ['#FF6B6B', '#FFE66D', '#4ECDC4']
        wedges, texts, autotexts = ax1.pie(priority_data.values, labels=priority_data.index, 
                                          autopct='%1.1f%%', colors=colors_pie, startangle=90)
        ax1.set_title('?? Task Distribution by Priority', fontsize=14, fontweight='bold', color='white')
        for text in texts + autotexts:
            text.set_color('white')
        
        # Chart 2: Time by priority bar chart
        priority_time = df.groupby('Priority')['TotalMinutes'].sum()
        bars = ax2.bar(priority_time.index, priority_time.values / 60, color=colors_pie)  # Convert to hours
        ax2.set_xlabel('Priority Level', fontsize=12, color='white')
        ax2.set_ylabel('Total Hours', fontsize=12, color='white')
        ax2.set_title('?? Total Time by Priority', fontsize=14, fontweight='bold', color='white')
        ax2.set_facecolor('#2d2d2d')
        ax2.tick_params(colors='white')
        
        # Add value labels on bars
        for bar in bars:
            height = bar.get_height()
            ax2.text(bar.get_x() + bar.get_width()/2., height + 0.5,
                    f'{height:.1f}h', ha='center', va='bottom', color='white', fontweight='bold')
        
        # Chart 3: Status distribution
        status_data = df.groupby('Status')['TaskCount'].sum()
        status_colors = ['#4CAF50', '#FF9800', '#F44336']
        bars = ax3.bar(status_data.index, status_data.values, color=status_colors)
        ax3.set_xlabel('Task Status', fontsize=12, color='white')
        ax3.set_ylabel('Number of Tasks', fontsize=12, color='white')
        ax3.set_title('?? Task Count by Status', fontsize=14, fontweight='bold', color='white')
        ax3.set_facecolor('#2d2d2d')
        ax3.tick_params(colors='white')
        
        # Add value labels
        for bar in bars:
            height = bar.get_height()
            ax3.text(bar.get_x() + bar.get_width()/2., height + 0.5,
                    f'{int(height)}', ha='center', va='bottom', color='white', fontweight='bold')
        
        # Chart 4: Average time per task by priority
        avg_time = df.groupby('Priority')['AvgMinutes'].mean()
        bars = ax4.bar(avg_time.index, avg_time.values, color=colors_pie)
        ax4.set_xlabel('Priority Level', fontsize=12, color='white')
        ax4.set_ylabel('Average Minutes per Task', fontsize=12, color='white')
        ax4.set_title('?? Average Time per Task by Priority', fontsize=14, fontweight='bold', color='white')
        ax4.set_facecolor('#2d2d2d')
        ax4.tick_params(colors='white')
        
        # Add value labels
        for bar in bars:
            height = bar.get_height()
            ax4.text(bar.get_x() + bar.get_width()/2., height + 1,
                    f'{height:.0f}m', ha='center', va='bottom', color='white', fontweight='bold')
        
        plt.tight_layout()
        plt.savefig(save_path, dpi=300, bbox_inches='tight', facecolor='#1a1a1a')
        plt.close()
        print(f"Time analysis chart saved to {save_path}")
    
    def create_daily_productivity_chart(self, days=30, save_path="daily_productivity.png"):
        """?? Create daily productivity trend chart"""
        query = f"""
        SELECT 
            CAST(CreatedAt AS DATE) as TaskDate,
            COUNT(*) as TasksCreated,
            COUNT(CASE WHEN Status = 'Completed' THEN 1 END) as TasksCompleted,
            AVG(CAST(EstimatedMinutes AS FLOAT)) as AvgEstimatedMinutes
        FROM Tasks 
        WHERE IsDeleted = 0 
            AND CreatedAt >= DATEADD(DAY, -{days}, GETDATE())
        GROUP BY CAST(CreatedAt AS DATE)
        ORDER BY TaskDate
        """
        
        df = self.get_data(query)
        if df.empty:
            print("No daily productivity data found")
            return
        
        # Convert TaskDate to datetime
        df['TaskDate'] = pd.to_datetime(df['TaskDate'])
        df['CompletionRate'] = (df['TasksCompleted'] / df['TasksCreated'] * 100).round(1)
        
        fig, (ax1, ax2, ax3) = plt.subplots(3, 1, figsize=(14, 12))
        fig.patch.set_facecolor('#1a1a1a')
        
        # Chart 1: Tasks created and completed over time
        ax1.plot(df['TaskDate'], df['TasksCreated'], marker='o', linewidth=2, 
                label='?? Tasks Created', color='#4ECDC4', markersize=6)
        ax1.plot(df['TaskDate'], df['TasksCompleted'], marker='s', linewidth=2, 
                label='? Tasks Completed', color='#4CAF50', markersize=6)
        ax1.fill_between(df['TaskDate'], df['TasksCompleted'], alpha=0.3, color='#4CAF50')
        
        ax1.set_xlabel('Date', fontsize=12, color='white')
        ax1.set_ylabel('Number of Tasks', fontsize=12, color='white')
        ax1.set_title('?? Daily Task Creation vs Completion', fontsize=14, fontweight='bold', color='white')
        ax1.legend()
        ax1.grid(True, alpha=0.3)
        ax1.set_facecolor('#2d2d2d')
        ax1.tick_params(colors='white')
        
        # Chart 2: Completion rate
        bars = ax2.bar(df['TaskDate'], df['CompletionRate'], 
                      color=['#4CAF50' if x >= 70 else '#FF9800' if x >= 30 else '#F44336' 
                            for x in df['CompletionRate']], alpha=0.8)
        ax2.set_xlabel('Date', fontsize=12, color='white')
        ax2.set_ylabel('Completion Rate (%)', fontsize=12, color='white')
        ax2.set_title('?? Daily Task Completion Rate', fontsize=14, fontweight='bold', color='white')
        ax2.set_ylim(0, 100)
        ax2.axhline(y=50, color='yellow', linestyle='--', alpha=0.7, label='50% Target')
        ax2.legend()
        ax2.set_facecolor('#2d2d2d')
        ax2.tick_params(colors='white')
        
        # Chart 3: Average estimated time per task
        ax3.plot(df['TaskDate'], df['AvgEstimatedMinutes'], marker='d', linewidth=2, 
                color='#FFE66D', markersize=6)
        ax3.fill_between(df['TaskDate'], df['AvgEstimatedMinutes'], alpha=0.3, color='#FFE66D')
        ax3.set_xlabel('Date', fontsize=12, color='white')
        ax3.set_ylabel('Average Minutes per Task', fontsize=12, color='white')
        ax3.set_title('?? Average Estimated Time per Task', fontsize=14, fontweight='bold', color='white')
        ax3.grid(True, alpha=0.3)
        ax3.set_facecolor('#2d2d2d')
        ax3.tick_params(colors='white')
        
        # Format x-axis dates
        for ax in [ax1, ax2, ax3]:
            ax.xaxis.set_major_formatter(mdates.DateFormatter('%m/%d'))
            ax.xaxis.set_major_locator(mdates.DayLocator(interval=max(1, len(df)//10)))
            plt.setp(ax.xaxis.get_majorticklabels(), rotation=45, color='white')
        
        plt.tight_layout()
        plt.savefig(save_path, dpi=300, bbox_inches='tight', facecolor='#1a1a1a')
        plt.close()
        print(f"Daily productivity chart saved to {save_path}")
    
    def create_interactive_dashboard(self, save_path="interactive_dashboard.html"):
        """?? Create interactive dashboard with Plotly"""
        import plotly.graph_objects as go
        from plotly.subplots import make_subplots
        import plotly.offline as pyo
        
        # Get comprehensive data
        query = """
        SELECT 
            p.ProjectName,
            t.Status,
            t.Priority,
            CAST(t.CreatedAt AS DATE) as CreatedDate,
            t.EstimatedMinutes,
            u.FullName
        FROM Tasks t
        JOIN Projects p ON t.ProjectID = p.ProjectID
        JOIN Users u ON t.UserID = u.UserID
        WHERE t.IsDeleted = 0 AND p.IsArchived = 0
        """
        
        df = self.get_data(query)
        if df.empty:
            print("No data for interactive dashboard")
            return
        
        # Create subplot with 4 charts
        fig = make_subplots(
            rows=2, cols=2,
            subplot_titles=['?? Task Status Distribution', '?? Priority Analysis', 
                          '?? Tasks Over Time', '?? Tasks by User'],
            specs=[[{"type": "pie"}, {"type": "bar"}],
                   [{"type": "scatter"}, {"type": "bar"}]]
        )
        
        # 1. Pie chart - Task Status
        status_counts = df['Status'].value_counts()
        fig.add_trace(
            go.Pie(labels=status_counts.index, values=status_counts.values,
                   name="Status", hole=0.3, 
                   marker_colors=['#4CAF50', '#FF9800', '#F44336']),
            row=1, col=1
        )
        
        # 2. Bar chart - Priority
        priority_counts = df['Priority'].value_counts()
        fig.add_trace(
            go.Bar(x=priority_counts.index, y=priority_counts.values,
                   name="Priority", 
                   marker_color=['#F44336', '#FF9800', '#4CAF50']),
            row=1, col=2
        )
        
        # 3. Time series - Tasks over time
        time_data = df.groupby('CreatedDate').size().reset_index(name='count')
        fig.add_trace(
            go.Scatter(x=time_data['CreatedDate'], y=time_data['count'],
                      mode='lines+markers', name="Tasks Created",
                      line=dict(color='#4ECDC4', width=3),
                      marker=dict(size=8)),
            row=2, col=1
        )
        
        # 4. Bar chart - Tasks by user
        user_counts = df['FullName'].value_counts().head(10)
        fig.add_trace(
            go.Bar(x=user_counts.index, y=user_counts.values,
                   name="User Tasks",
                   marker_color='#96CEB4'),
            row=2, col=2
        )
        
        # Update layout for dark theme
        fig.update_layout(
            height=700,
            showlegend=False,
            title_text="?? ToDoList Interactive Analytics Dashboard",
            title_font_size=20,
            title_font_color='white',
            paper_bgcolor='#1a1a1a',
            plot_bgcolor='#2d2d2d',
            font_color='white'
        )
        
        # Update axes colors
        fig.update_xaxes(color='white', gridcolor='#404040')
        fig.update_yaxes(color='white', gridcolor='#404040')
        
        # Save as HTML
        pyo.plot(fig, filename=save_path, auto_open=False)
        print(f"Interactive dashboard saved to {save_path}")
    
    def generate_all_charts(self, output_dir="charts"):
        """Generate all charts at once"""
        import os
        
        if not os.path.exists(output_dir):
            os.makedirs(output_dir)
        
        if not self.connect_database():
            return
        
        print("?? Generating beautiful charts with Python...")
        
        try:
            self.create_project_progress_chart(f"{output_dir}/project_progress.png")
            self.create_time_analysis_chart(f"{output_dir}/time_analysis.png")
            self.create_daily_productivity_chart(30, f"{output_dir}/daily_productivity.png")
            self.create_interactive_dashboard(f"{output_dir}/interactive_dashboard.html")
            
            print("? All charts generated successfully!")
            print(f"?? Charts saved in '{output_dir}' folder")
            
        except Exception as e:
            print(f"? Error generating charts: {e}")
        finally:
            if hasattr(self, 'connection'):
                self.connection.close()

# Usage
if __name__ == "__main__":
    try:
        # Auto-detect SQL Server instance
        import socket
        hostname = socket.gethostname()
        
        # Try different connection strings
        connection_strings = [
            f"DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={hostname}\\SQLEXPRESS;DATABASE=ToDoListApp;Trusted_Connection=yes;",
            f"DRIVER={{SQL Server}};SERVER={hostname}\\SQLEXPRESS;DATABASE=ToDoListApp;Trusted_Connection=yes;",
            "DRIVER={ODBC Driver 17 for SQL Server};SERVER=localhost\\SQLEXPRESS;DATABASE=ToDoListApp;Trusted_Connection=yes;",
            "DRIVER={SQL Server};SERVER=localhost\\SQLEXPRESS;DATABASE=ToDoListApp;Trusted_Connection=yes;",
        ]
        
        print("?? Auto-detecting SQL Server connection...")
        chart_gen = None
        
        for i, conn_str in enumerate(connection_strings):
            try:
                print(f"   Trying connection {i+1}...")
                chart_gen = ToDoListChartGenerator(conn_str)
                if chart_gen.connect_database():
                    print(f"? Connected successfully with connection {i+1}")
                    break
            except Exception as e:
                print(f"   ? Connection {i+1} failed: {e}")
                continue
        
        if chart_gen is None:
            print("? Could not connect to database with any connection string")
            print("\n?? Please check:")
            print("1. SQL Server is running")
            print("2. Database 'ToDoListApp' exists") 
            print("3. ODBC drivers are installed")
            input("\nPress Enter to exit...")
            exit(1)
        
        # Generate all charts
        print("\n?? Generating charts...")
        chart_gen.generate_all_charts("ToDoList_Charts")
        
    except Exception as e:
        print(f"? Fatal error: {e}")
        import traceback
        traceback.print_exc()
        input("\nPress Enter to exit...")
        exit(1)