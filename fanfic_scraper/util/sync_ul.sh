rclone sync -v /opt/home/ubuntu/OneDrive/FF_Archive onedrive:FF_Archive 
rclone rmdirs --leave-root -v onedrive:FF_Archive
rclone sync -v /opt/home/ubuntu/OneDrive/FF_Archive/Current_FF onedrive:Fanfiction
rclone rmdirs --leave-root -v onedrive:Fanfiction 
