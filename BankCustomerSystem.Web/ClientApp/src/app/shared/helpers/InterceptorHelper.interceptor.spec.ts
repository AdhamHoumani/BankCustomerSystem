import { TestBed } from '@angular/core/testing';

import { InterceptorHelper } from './InterceptorHelper.interceptor';

describe('InterceptorHelperInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      InterceptorHelper
      ]
  }));

  it('should be created', () => {
    const interceptor: InterceptorHelper = TestBed.inject(InterceptorHelper);
    expect(interceptor).toBeTruthy();
  });
});
