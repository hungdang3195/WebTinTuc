import { Component, OnInit, ViewChild, TemplateRef, ElementRef } from "@angular/core";
import { LookupItem, FileViewModel, Address, Location, ProductImage } from '../../models';
import { Observable } from 'rxjs';
import { AkcTableOption, AkcTableConstant, AkcTableComponent } from '../../controls/table';
import { ClientValidationOption, ClientValidationProvider, ClientValidator, ValidationConstant } from '../../validations';
import { AkcModalService, ModalServiceConstant } from '../../controls';
import { GridComponent } from './grid.component';
import { TablePageComponent } from '..';

@Component({
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss'],
})
export class DashboardPageComponent implements OnInit {
  @ViewChild('form') public formRef: ElementRef;
  @ViewChild('createdDateCustomTemplate', { read: TemplateRef }) public createdDateCustomTemplate: TemplateRef<any>;
  @ViewChild('nameCustomTemplate', { read: TemplateRef }) public nameCustomTemplate: TemplateRef<any>;
  @ViewChild('imageCustomTemplate', { read: TemplateRef }) public imageCustomTemplate: TemplateRef<any>;

  public tableOption: AkcTableOption = {
    showOrder: true,
    showFilter: true,
    showToolbar: true,
    totalToolbarItem: 2,
    localData: () => {
      let items = [];
      for (let i = 0; i < 120; i++) {
        items.push({
          id: i,
          order: i * 10000,
          time: '00:30',
          name: 'Danh mục ' + (i + 1),
          type: 'B2B' + (i + 1),
          description: 'Không có thông tin gì ' + (i + 1),
          enabled: i % 2 == 0,
          createdDate: new Date(2018, 11, i % 27 + 1, 0, 0, 0),
          image: {src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Gull_portrait_ca_usa.jpg/1280px-Gull_portrait_ca_usa.jpg'}
        });
      }
      return items;
    },
    editInline: {
      enabled: true,
      autoCommit: true,
      updateAsync: (item: any) => {
        return Observable.of([]).delay(2000);
      },
    },
    actions: [
      {
        tooltip: () => "Một thanh sắt trên đường Trần Duy Hưng (Hà Nội) đã bất ngờ rơi xuống...",
        icon: 'fa fa-exchange',
        customClass: 'text-primary',
        executeAsync: (provider, item, element) => {
          this._akcModalService.showConfirmDialog({
            message: 'test',
            title: 'test',
          })
        },
        type: AkcTableConstant.ActionType.Toolbar
      }
    ],
    topButtons: [
      {
        tooltip: () => "Một thanh sắt trên đường Trần Duy Hưng (Hà Nội) đã bất ngờ rơi xuống...",
        title: () => "Thêm mới",
        icon: 'fa fa-plus',
        customClass: 'btn-link text-uppercase',
        executeAsync: (item, element) => {
          return this._akcModalService.showTemplateDialog({
            title: 'Thêm mới',
            mode: ModalServiceConstant.Mode.Create,
            autoClose: true,
            btnAcceptTitle: 'Thêm mới',
            btnCancelTitle: 'Hủy',
            template: AkcTableComponent,
            data: {
              option: this.tableOption
            },
            acceptCallback: (response) => {
              //todo
              console.log(response);
            }
          });
        },
        disabled: true
      },
      {
        tooltip: () => "Một thanh sắt trên đường Trần Duy Hưng (USA) đã bất ngờ rơi xuống...",
        title: () => "Chỉnh sửa",
        icon: 'fa fa-pencil',
        customClass: 'btn-info',
        executeAsync: (provider, item, element) => {
          return this._akcModalService.showTemplateDialog({
            title: 'Thêm mới',
            mode: ModalServiceConstant.Mode.Custom,
            autoClose: true,
            btnAcceptTitle: 'Thêm mới',
            btnCancelTitle: 'Hủy',
            template: GridComponent,
            acceptCallback: (response) => {
              //todo
              console.log(response);
            }
          });
        }
      }
    ],
    detailColumns: [
      {
        title: () => 'Ngày tạo',
        valueRef: () => "createdDate",
        type: AkcTableConstant.Type.Date,
        customClass: 'col-md-3 col-xs-12',
        allowFilter: true,
        order: 1,
      },
      {
        title: () => 'Chú thích',
        valueRef: () => "description",
        type: AkcTableConstant.Type.Description,
        allowFilter: true,
        order: 1
      },
      {
        title: () => 'Áp dụng?',
        valueRef: () => "enabled",
        type: AkcTableConstant.Type.Boolean,
        textAlign: AkcTableConstant.TextAlign.Center,
        customClass: 'col-md-3 col-xs-12',
        allowFilter: true,
        order: 2
      },
    ],
    mainColumns: [
      {
        title: () => 'Tên danh mục',
        valueRef: () => "name",
        allowFilter: true,
        allowSort: true,
        direction: AkcTableConstant.Direction.DESC,
        width: 250,
        type: AkcTableConstant.Type.String,
        editInline: true,
        order: 1,
        customTemplate: () => this.nameCustomTemplate
      },
      {
        title: () => 'Ảnh',
        valueRef: () => "image",
        allowFilter: true,
        allowSort: true,
        direction: AkcTableConstant.Direction.DESC,
        // width: 250,
        type: AkcTableConstant.Type.String,
        editInline: true,
        order: 1,
        customTemplate: () => this.imageCustomTemplate
      },
      {
        title: () => 'Loại danh mục',
        valueRef: () => "type",
        defaultSorter: false,
        direction: AkcTableConstant.Direction.ASC,
        width: 150,
        allowFilter: true,
        allowSort: true,
        textAlign: AkcTableConstant.TextAlign.Center,
        type: AkcTableConstant.Type.String,
        editInline: false,
        order: 1
      },
      {
        title: () => 'Áp dụng?',
        valueRef: () => "enabled",
        type: AkcTableConstant.Type.Boolean,
        textAlign: AkcTableConstant.TextAlign.Center,
        customClass: 'col-md-3 col-xs-12',
        allowFilter: true,
        order: 2
      },
      {
        title: () => 'Thứ tự',
        valueRef: () => "order",
        type: AkcTableConstant.Type.Number,
        textAlign: AkcTableConstant.TextAlign.Center,
        customClass: 'col-md-3 col-xs-12',
        allowFilter: true,
        order: 2
      },
      {
        title: () => 'Ngày tạo',
        valueRef: () => "createdDate",
        type: AkcTableConstant.Type.Date,
        customClass: 'col-md-3 col-xs-12',
        order: 1,
        customTemplate: () => this.createdDateCustomTemplate
      },
      {
        title: () => 'Thời gian',
        valueRef: () => "time",
        type: AkcTableConstant.Type.Time,
        customClass: 'col-md-3 col-xs-12',
        order: 1
      }
    ]
  };
  public gridOption: AkcTableOption = {
    showOrder: true,
    showFilter: false,
    showToolbar: true,
    localData: () => {
      let items = [];
      for (let i = 0; i < 50; i++) {
        items.push({
          id: i,
          order: i * 10000,
          time: '00:30',
          name: 'Danh mục ' + (i + 1),
          type: 'B2B' + (i + 1),
          description: 'Không có thông tin gì ' + (i + 1),
          enabled: i % 2 == 0,
          createdDate: new Date(2018, 11, i % 27 + 1, 0, 0, 0)
        });
      }
      return items;
    },
    topButtons: [
      {
        title: () => "Thêm mới",
        icon: 'fa fa-plus',
        customClass: 'btn-primary',
        executeAsync: (item, element) => {
          return this._akcModalService.showTemplateDialog({
            title: 'Thêm mới',
            mode: ModalServiceConstant.Mode.Create,
            autoClose: true,
            btnAcceptTitle: 'Thêm mới',
            btnCancelTitle: 'Hủy',
            template: GridComponent,
            data: {
              option: this.tableOption
            },
            acceptCallback: (response) => {
              //todo
              console.log(response);
            }
          });
        },
        disabled: false
      },
      {
        tooltip: () => "Một thanh sắt trên đường Trần Duy Hưng (USA) đã bất ngờ rơi xuống...",
        title: () => "Chỉnh sửa",
        icon: 'fa fa-pencil',
        customClass: 'btn-info',
        executeAsync: (provider, item, element) => {
          return this._akcModalService.showTemplateDialog({
            title: 'Thêm mới',
            mode: ModalServiceConstant.Mode.Custom,
            autoClose: true,
            btnAcceptTitle: 'Thêm mới',
            btnCancelTitle: 'Hủy',
            template: GridComponent,
            acceptCallback: (response) => {
              //todo
              console.log(response);
            }
          });
        }
      }
    ],
    mainColumns: [
      {
        title: () => 'Tên danh mục',
        valueRef: () => "name",
        allowFilter: true,
        allowSort: true,
        direction: AkcTableConstant.Direction.DESC,
        width: 250,
        type: AkcTableConstant.Type.String,
        editInline: true,
        order: 1,
        customTemplate: () => this.nameCustomTemplate
      },
      {
        title: () => 'Loại danh mục',
        valueRef: () => "type",
        defaultSorter: false,
        direction: AkcTableConstant.Direction.ASC,
        width: 150,
        allowFilter: true,
        allowSort: true,
        textAlign: AkcTableConstant.TextAlign.Center,
        type: AkcTableConstant.Type.String,
        editInline: false,
        order: 1
      },
      {
        title: () => 'Áp dụng?',
        valueRef: () => "enabled",
        type: AkcTableConstant.Type.Boolean,
        textAlign: AkcTableConstant.TextAlign.Center,
        customClass: 'col-md-3 col-xs-12',
        allowFilter: true,
        order: 2
      },
      {
        title: () => 'Thứ tự',
        valueRef: () => "order",
        type: AkcTableConstant.Type.Number,
        textAlign: AkcTableConstant.TextAlign.Center,
        customClass: 'col-md-3 col-xs-12',
        allowFilter: true,
        order: 2
      },
      {
        title: () => 'Ngày tạo',
        valueRef: () => "createdDate",
        type: AkcTableConstant.Type.Date,
        customClass: 'col-md-3 col-xs-12',
        order: 1,
        customTemplate: () => this.createdDateCustomTemplate
      },
      {
        title: () => 'Thời gian',
        valueRef: () => "time",
        type: AkcTableConstant.Type.Time,
        customClass: 'col-md-3 col-xs-12',
        order: 1
      }
    ]
  };
  public loading: boolean;

  public dropdownItems: LookupItem[] = [];
  public dropdownViewModel: any;

  public checkboxViewModel: boolean;
  public customCheckboxViewModel: string = 'show';
  public customCheckboxModelTransformation = (value: any) => {
    if (value === 'show') return true;
    if (value === 'hide') return false;
    if (value === true) return 'show';
    if (value === false) return 'hide';
    return null;
  }

  public textboxViewModel: string;
  public numberViewModel: number = 10000000000000000;

  public dateTimePickerViewModel: Date;
  public dateRangePickerViewModel: Date[];

  public radioViewModel: any = 'b';

  public fileUploaderViewModel: string;

  public chipsViewModel: string[] = ['Item 1', 'Item 2'];

  public editorViewModel: string = 'Hello';

  public textAreaViewModel: string = 'Hi';

  public validationOptions: ClientValidationOption[] = [
    {
      validationName: 'testValidation',
      valueRef: () => this.textboxViewModel,
      actions: [
        {
          key: ValidationConstant.Required,
          errorMessage: () => 'Không được để trống.'
        },

        {
          key: ValidationConstant.Custom,
          errorMessage: () => "Giá trị phải = 123",
          execute: () => {
            if (this.textboxViewModel == "123") {
              return true;
            }
            return false;
          }
        },
        {
          key: ValidationConstant.Custom,
          executeAsync: (commit) => {
            commit(false, 'Không hợp lệ request server.');
          },
          isServerRequest: true
        },
      ],
    },
    { validationName: 'iterations', valueRef: () => this.iterationsDropdownViewModel, actions: [{ key: ValidationConstant.Required, errorMessage: () => 'Không được để trống.' }] }
  ];

  public categoriesDropdownViewModel: LookupItem[] = [];
  public iterationsDropdownViewModel: LookupItem[] = [];
  public originsDropdownViewModel: LookupItem[] = [];
  public unitsDropdownViewModel: LookupItem[] = [];
  public searchLookupItems$: Observable<LookupItem[]>;
  public searchCategories = (searchText: string, index: number, pageIndex: number, pageSize: number) => {
    console.info(`searchCategories`);
    const records = this.lookupItems.filter(x => x.key === 'Category');
    const result = Observable.of({
      items: records.filter(x => x.name.includes(searchText)),
      totalRecords: records.length
    }).delay(1500);
    return result;
  };
  public searchIterations = (searchText: string, index: number, pageIndex: number, pageSize: number) => {
    console.info(`searchIterations`);
    const records = this.lookupItems.filter(x => x.key === 'Iteration');
    return Observable.of({
      items: records.filter(x => x.name.includes(searchText)),
      totalRecords: records.length
    }).delay(1500);
  };
  public searchOrigins = (searchText: string, index: number, pageIndex: number, pageSize: number) => {
    console.info(`searchOrigins`);
    const records = this.lookupItems.filter(x => x.key === 'Origin');
    return Observable.of({
      items: records.filter(x => x.name.includes(searchText)).filter(x => records.indexOf(x) >= pageIndex * pageSize && records.indexOf(x) < (pageIndex + 1) * pageSize),
      totalRecords: records.length
    }).delay(1500);
  };
  public searchUnits = (searchText: string, index: number, pageIndex: number, pageSize: number) => {
    console.info(`searchUnits`);
    const records = this.lookupItems.filter(x => x.key === 'Unit');
    return Observable.of({
      items: records.filter(x => x.name.includes(searchText)).filter(x => records.indexOf(x) >= pageIndex * pageSize && records.indexOf(x) < (pageIndex + 1) * pageSize),
      totalRecords: records.length
    }).delay(1500);
  };

  public listViewModel = [];
  public addressViewModel: Address = new Address();
  public locations: Location[] = [];

  public imageViewModel: ProductImage = new ProductImage({
    src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Gull_portrait_ca_usa.jpg/1280px-Gull_portrait_ca_usa.jpg'
  });
  public emptyImageViewModel: ProductImage = new ProductImage({
    src: ''
  });

  public imageGalleryViewModel: ProductImage[] = [
    new ProductImage({ src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Gull_portrait_ca_usa.jpg/1280px-Gull_portrait_ca_usa.jpg' }),
    new ProductImage({ src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Gull_portrait_ca_usa.jpg/1280px-Gull_portrait_ca_usa.jpg' }),
    new ProductImage({ src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Gull_portrait_ca_usa.jpg/1280px-Gull_portrait_ca_usa.jpg' }),
    new ProductImage({ src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Gull_portrait_ca_usa.jpg/1280px-Gull_portrait_ca_usa.jpg' }),
  ];
  public emptyImageGalleryViewModel = [];

  private searchLookupItems = () => {
    console.info(`searchLookupItems`);
    let result: any[] = [];
    for (let index = 0; index < 100; index++) {
      result.push({ id: index.toString(), key: 'Category', name: `Category ${index}`, avatar: 'https://stepupandlive.files.wordpress.com/2014/09/3d-animated-frog-image.jpg', group: (index % 2).toString() });
      result.push(new LookupItem({ id: index.toString(), key: 'Iteration', name: `Iteration ${index}` }));
      result.push(new LookupItem({ id: index.toString(), key: 'Origin', name: `Origin ${index}` }));
      result.push(new LookupItem({ id: index.toString(), key: 'Unit', name: `Unit ${index}` }));

      this.locations.push({ name: `City ${index}`, level: 1, id: '', code: `city${index}`, parent: null, path: null, slug: null, type: null });
      this.locations.push({ name: `District ${index}`, level: 2, id: '', code: `city${index}_district${index}`, parent: null, path: null, slug: null, type: null });
      this.locations.push({ name: `Ward ${index}`, level: 3, id: '', code: `city${index}_district${index}_ward${index}`, parent: null, path: null, slug: null, type: null });
    }
    return Observable.of(result).delay(1500);
  };
  private lookupItems: LookupItem[] = [];

  constructor(
    private validationService: ClientValidationProvider,
    private _akcModalService: AkcModalService,
  ) { }

  ngOnInit() {
    console.log(this.createdDateCustomTemplate);
    this.searchLookupItems$ = Observable.from(this.searchLookupItems());
    this.searchLookupItems$.subscribe(response => {
      console.info(`this.searchLookupItems().subscribe`);
      this.lookupItems = response;
    });
    for (let index = 0; index < 100; index++) {
      this.dropdownItems.push(new LookupItem({ id: index.toString(), name: `Vật liêu ${index}` }));
    }
    for (let index = 0; index < 10; index++) {
      this.listViewModel.push({ id: index.toString(), name: `Item ${index}` });
    }
  }

  ngAfterViewInit() {
    const validator: ClientValidator = {
      formRef: this.formRef,
      options: this.validationOptions,
      enabledKeyupEvent: true
    }
    this.validationService.init(validator);
  }

  public buttonClicked(loadedCallback: Function) {
    setTimeout(() => {
      loadedCallback();
    }, 2000);
  }

  public searchDropdownItems = (text, index, currentPage: number, pageSize: number) => {
    let result = [].concat([...this.dropdownItems]);

    if (text) {
      result = result.filter(x => x.name.indexOf(text) >= 0);
    }
    if (currentPage >= 0 && pageSize > 0) {
      result = result.filter(x => (this.dropdownItems.indexOf(x) >= currentPage * pageSize)
        && (this.dropdownItems.indexOf(x) < (currentPage + 1) * pageSize));
    }

    return Observable.of({ items: [].concat([...result]), totalRecords: this.dropdownItems.length }).delay(3000);
  }

  public fileUploaded(file: FileViewModel) {
    this.fileUploaderViewModel = 'https://cdn/fake.jpg';
  }

  public notificationViewModel: any;
  public showNotification(): void {
    this._akcModalService.showNotificationDialog({
      autoClose: true,
      btnTitle: 'OK',
      callback: () => {
        this.notificationViewModel = {
          data: 'Click OK.'
        }
      },
      message: 'Thông báo từ hệ thống.',
      title: 'Thông báo'
    });
  }
  public confirmationViewModel: any;
  public showConfirmation(): void {
    this._akcModalService.showConfirmDialog({
      autoClose: true,
      btnAcceptTitle: 'Đồng ý',
      btnCancelTitle: 'Hủy bỏ',
      isDeleted: true,
      acceptCallback: () => {
        this.confirmationViewModel = {
          data: 'Click Accept.'
        };
      },
      cancelCallback: () => {
        this.confirmationViewModel = {
          data: 'Click Cancel.'
        };
      },
      message: 'Bạn có chắc chắn muốn xóa không?',
      title: 'Confirmation'
    });
  }

  public templateViewModel: any;
  public showTemplate(): void {
    this._akcModalService.showTemplateDialog({
      autoClose: true,
      mode: ModalServiceConstant.Mode.Picker,
      btnAcceptTitle: 'Đồng ý',
      btnCancelTitle: 'Hủy bỏ',
      hideCancelButton: true,
      acceptCallback: (response?: any) => {
        this.templateViewModel = {
          selectedItems: response
        };
        console.log(response);
      },
      cancelCallback: () => {
        this.templateViewModel = {
          data: 'Click Template Cancel.'
        };
      },
      template: TablePageComponent,
      data: {
        option: this.tableOption
      },
      title: 'Template',
      extraButtons: [
        {displayText: 'Button 1', lazyload: true, execute: (componentRef: TablePageComponent, loadedCallback: Function) => {
          componentRef.test();
          loadedCallback();
        }, hide: (componentRef: TablePageComponent) => {
          const result = componentRef.hideTest();
          return result;
        }},
        {displayText: 'Button 2', execute: () => {
        }}
      ]
    });
  }

  public selectCategory(item: any) {
    console.log(item.name);
  }
}
