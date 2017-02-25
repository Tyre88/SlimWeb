import { SlimWebClientPage } from './app.po';

describe('slim-web-client App', () => {
  let page: SlimWebClientPage;

  beforeEach(() => {
    page = new SlimWebClientPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
