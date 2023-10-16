import { CurrencyPipe, JsonPipe, NgFor, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject,ViewEncapsulation } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    FormsModule,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {  MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule, MatCalendarCellClassFunction } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableDataSource } from '@angular/material/table';

interface Room {
  id?: string;
  name?: string;
  description?: string;
  address?: string;
  capacity: number;
  price: number;
  images?: string[];
  bookings?: Booking[];
}

export interface Booking {
  id: string;
  room: Room;
  startDate: Date;
  endDate: Date;
  email: string;
  numberOfPeople: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent{
  public rooms: Room[] = [];
  displayedColumns: string[] = ['name', 'description', 'address', 'capacity', 'price', 'action'];

  dataSource = new MatTableDataSource<Room>(this.rooms);

  constructor(private http: HttpClient, public dialog: MatDialog) {
    this.http.get<Room[]>('/api/Room/all').subscribe(
      (result) => {
        this.rooms = result;
        this.dataSource.data = this.rooms;
      },
      (error) => console.error(error)
    );
  }

  openDialog(room:Room): void {
    const dialogRef = this.dialog.open(BookingDialog,{
      data: { room: room }
    });

    dialogRef.afterClosed().subscribe();
  }

  title = 'BookingManagerApp';
}

@Component({
  selector: 'booking-dialog',
  templateUrl: 'booking-dialog.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None,
  standalone: true,
  imports: [
    FormsModule,
    MatButtonModule,
    MatDatepickerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    NgFor,
    JsonPipe,
    CurrencyPipe,
    MatNativeDateModule,
  ],
  providers: [
    MatDatepickerModule,
  ],
})
export class BookingDialog {
  bookingForm: FormGroup;
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  numberFormControl = new FormControl('', [Validators.required, Validators.max(this.data.room.capacity)]);
  startDateFormControl = new FormControl<Date | null>(null);
  endDateFormControl =  new FormControl<Date | null>(null);
  dateClass: MatCalendarCellClassFunction<Date> = (cellDate) => {
    if (this.data.room.bookings) {
      const cellDateOnly = new Date(cellDate.getFullYear(), cellDate.getMonth(), cellDate.getDate());

      const isBooked = this.data.room.bookings.some((booking) => {
        const startDate = new Date(booking.startDate);
        const endDate = new Date(booking.endDate);
        return cellDateOnly >= startDate && cellDateOnly <= endDate;
      });

      if (isBooked) return 'booked-date-class';
    }
    return '';
  }

  public bookingSuccess: boolean = false;
  public bookingError: boolean = false;

  constructor(
    private http: HttpClient,
    public dialogRef: MatDialogRef<BookingDialog>,
    @Inject(MAT_DIALOG_DATA) public data: Booking,
    private formBuilder: FormBuilder
  ) {
    this.bookingForm = this.formBuilder.group({
      emailFormControl: ['', [Validators.required, Validators.email]],
      numberFormControl: ['', [Validators.required, Validators.min(1), Validators.max(this.data.room.capacity)]],
      startDateFormControl: this.startDateFormControl,
      endDateFormControl: this.endDateFormControl,
    });
  }

  onNoClick(): void { this.dialogRef.close(); }

  onSubmit() {
    this.bookingError = false;
    this.bookingSuccess = false;

    if (BookingDialog.checkIfDateRangeContainsBookings(this.startDateFormControl.value, this.endDateFormControl.value, this.data.room.bookings))
      this.startDateFormControl.setErrors({ 'dateRangeContainsBookings': true });
    else
      this.startDateFormControl.setErrors(null);

    if(!this.bookingForm.valid) return;

    let values = this.bookingForm.value;
    let formData = {
      Id: '',
      Email: values.emailFormControl,
      StartDate: values.startDateFormControl,
      EndDate: values.endDateFormControl,
      NumberOfPeople: values.numberFormControl,
      RoomId: this.data.room.id,
    };

    this.http.post('/api/Booking', formData).subscribe((result: any) => {
      this.data.room = result;
        console.log(result)
        this.bookingSuccess = true;
    },
      (error) => {
      this.bookingError = true;
      console.error(error)
    });
  }

  private static checkIfDateRangeContainsBookings(selectedStartDate: Date | null, selectedEndDate: Date | null, bookings: Booking[] | undefined) : boolean
  {
    const startDate = selectedStartDate ?? new Date(0);

    if (startDate < new Date()) return true;
    if (bookings == undefined) return false;

    const endDate = selectedEndDate ?? new Date();

    for (const booking of bookings) {
      const bookingStartDate = new Date(booking.startDate);
      const bookingEndDate = new Date(booking.endDate);

      if (
        (startDate >= bookingStartDate && startDate <= bookingEndDate) ||
        (endDate >= bookingStartDate && endDate <= bookingEndDate) ||
        (startDate <= bookingStartDate && endDate >= bookingEndDate)
      ) {
        return true;
      }
    }
    return false;
  }
}
